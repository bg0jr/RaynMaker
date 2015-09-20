using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Import.Spec;
using RaynMaker.Infrastructure;

namespace RaynMaker.Import.Web.Services
{
    [Export]
    class StorageService
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public StorageService( IProjectHost projectHost )
        {
            myProjectHost = projectHost;
        }

        public IEnumerable<DatumLocator> Load()
        {
            var file = Path.Combine( myProjectHost.Project.StorageRoot, "DatumLocators.xdb" );

            if( !File.Exists( file ) )
            {
                return Enumerable.Empty<DatumLocator>();
            }

            var sheet = Load( file );

            Migrate( sheet );

            RecursiveValidator.Validate( sheet );

            return sheet.Locators;
        }

        private static DatumLocatorSheet Load( string file )
        {
            using( var stream = new FileStream( file, FileMode.Open, FileAccess.Read ) )
            {
                var serializer = CreateSerializer();
                return ( DatumLocatorSheet )serializer.ReadObject( stream );
            }
        }

        private void Migrate( DatumLocatorSheet sheet )
        {
            foreach( var locator in sheet.Locators )
            {
                foreach( var site in locator.Sites )
                {
                    foreach( var format in site.Formats )
                    {
                        format.Datum = locator.Datum;
                    }
                }
            }

            RecursiveValidator.Validate( sheet );

            Store( sheet.Locators );
        }

        public void Store( IEnumerable<DatumLocator> locators )
        {
            var file = Path.Combine( myProjectHost.Project.StorageRoot, "DatumLocators.xdb" );
            using( var stream = new FileStream( file, FileMode.Create, FileAccess.Write ) )
            {
                var serializer = CreateSerializer();
                serializer.WriteObject( stream, new DatumLocatorSheet { Locators = locators } );
            }
        }

        private static DataContractSerializer CreateSerializer()
        {
            var settings = new DataContractSerializerSettings();
            settings.KnownTypes = new[] { typeof( DatumLocatorSheet ) }
                .Concat( typeof( DatumLocator ).Assembly.GetTypes()
                    .Where( t => !t.IsAbstract )
                    .Where( t => t.GetCustomAttributes( false ).OfType<DataContractAttribute>().Any() ) )
                .ToList();

            var serializer = new DataContractSerializer( typeof( DatumLocator ), settings );
            return serializer;
        }
    }
}
