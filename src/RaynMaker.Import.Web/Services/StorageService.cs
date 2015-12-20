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

        // TODO: add some caching here to avoid Disk/IO
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

            return Enumerable.Empty<DataSource>();
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
            settings.KnownTypes = typeof( DataSource ).Assembly.GetTypes()
                .Where( t => !t.IsAbstract )
                .Where( t => t.GetCustomAttributes( false ).OfType<DataContractAttribute>().Any() )
                .ToList();

            return new DataContractSerializer( root, settings );
        }
    }
}
