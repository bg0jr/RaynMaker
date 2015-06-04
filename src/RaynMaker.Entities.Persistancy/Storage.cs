using System.ComponentModel.Composition;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Plainion;

namespace RaynMaker.Entities.Persistancy
{
    public class Storage : IContextFactory
    {
        private const int RequiredDatabaseVersion = 1;
        private static bool myIsInitialized;

        public Storage( string location )
        {
            Contract.RequiresNotNullNotEmpty( location, "location" );

            Location = location;
        }

        public string Location { get; private set; }

        public void Initialize()
        {
            SchemaInfo schemaInfo = null;

            if( !File.Exists( Location ) )
            {
                SQLiteConnection.CreateFile( Location );
                schemaInfo = new SchemaInfo { Id = -1, Version = 0 };
            }

            using( var context = new AssetsContext( Location ) )
            {
                if( schemaInfo == null )
                {
                    schemaInfo = context.SchemaInfos.Single();
                }

                var migrationScript = new DBMigrationScript();
                while( schemaInfo.Version < RequiredDatabaseVersion )
                {
                    schemaInfo.Version++;
                    foreach( string migration in migrationScript.Migrations[ schemaInfo.Version ] )
                    {
                        context.Database.ExecuteSqlCommand( migration );
                    }
                    context.SaveChanges();
                }

                if( schemaInfo.Id == -1 )
                {
                    context.SchemaInfos.Add( schemaInfo );
                    context.SaveChanges();
                }
            }

            myIsInitialized = true;
        }

        public IAssetsContext CreateAssetsContext()
        {
            Contract.Invariant( myIsInitialized, "Initialized() not yet called" );

            return new AssetsContext( Location );
        }
    }
}
