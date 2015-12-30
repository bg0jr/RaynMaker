using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2;
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
                    var serializer = new ImportSpecSerializer();
                    var sheet = serializer.Read<DataSourcesSheet>( stream );
                    return sheet.Sources;
                }
            }

            return Enumerable.Empty<DataSource>();
        }

        public void Store( IEnumerable<DataSource> sources )
        {
            var sheet = new DataSourcesSheet();
            sheet.Sources = sources;

            var file = Path.Combine( myProjectHost.Project.StorageRoot, "DataSources.xdb" );
            using( var stream = new FileStream( file, FileMode.Create, FileAccess.Write ) )
            {
                var serializer = new ImportSpecSerializer();
                serializer.Write( stream, sheet );
            }
        }
    }
}
