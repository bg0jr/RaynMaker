using System;
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

        public IEnumerable<DataSource> Load()
        {
            var file = Path.Combine( myProjectHost.Project.StorageRoot, "DataSources.xdb" );
            if( File.Exists( file ) )
            {
                using( var stream = new FileStream( file, FileMode.Open, FileAccess.Read ) )
                {
                    var serializer = CreateSerializer( typeof( DataSourcesSheet ) );
                    var sheet = ( DataSourcesSheet )serializer.ReadObject( stream );
                    return sheet.Sources;
                }
            }

            file = Path.Combine( myProjectHost.Project.StorageRoot, "DatumLocators.xdb" );

            if( !File.Exists( file ) )
            {
                return Enumerable.Empty<DataSource>();
            }

            IEnumerable<DatumLocator> locators;
            using( var stream = new FileStream( file, FileMode.Open, FileAccess.Read ) )
            {
                var serializer = CreateSerializer( typeof( DatumLocator ) );
                var sheet = ( DatumLocatorSheet )serializer.ReadObject( stream );
                locators = sheet.Locators;
            }

            File.Delete( file );

            return Migrate( locators );
        }

        private IEnumerable<DataSource> Migrate( IEnumerable<DatumLocator> locators )
        {
            var sources = new List<DataSource>();

            foreach( var locator in locators )
            {
                foreach( var site in locator.Sites )
                {
                    foreach( var format in site.Formats )
                    {
                        format.Datum = locator.Datum;
                    }

                    sources.Add( new DataSource
                    {
                        Vendor = site.Name,
                        Name = site.Name,
                        Quality = 1,
                        LocationSpec = site.Navigation,
                        FormatSpecs = site.Formats
                    } );
                }
            }

            Store( sources );

            return sources;
        }

        public void Store( IEnumerable<DataSource> sources )
        {
            var sheet = new DataSourcesSheet();
            sheet.Sources = sources;

            RecursiveValidator.Validate( sheet );

            var file = Path.Combine( myProjectHost.Project.StorageRoot, "DataSources.xdb" );
            using( var stream = new FileStream( file, FileMode.Create, FileAccess.Write ) )
            {
                var serializer = CreateSerializer( typeof( DataSourcesSheet ) );
                serializer.WriteObject( stream, sheet );
            }
        }

        private static DataContractSerializer CreateSerializer( Type root )
        {
            var settings = new DataContractSerializerSettings();
            settings.KnownTypes = new[] { typeof( DatumLocatorSheet ), typeof( DatumLocator ), typeof( Site ) }
                .Concat( typeof( DataSource ).Assembly.GetTypes()
                    .Where( t => !t.IsAbstract )
                    .Where( t => t.GetCustomAttributes( false ).OfType<DataContractAttribute>().Any() ) )
                .ToList();

            return new DataContractSerializer( root, settings );
        }
    }
}
