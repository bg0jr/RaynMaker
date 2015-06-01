using System.ComponentModel.Composition;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace RaynMaker.Entities.Persistancy
{
    [Export( typeof( IEntitiesContextFactory ) )]
    class EntitiesContextFactory : IEntitiesContextFactory
    {
        private static bool myIsDatabaseInitialized;

        public IEntityContext Create( string path )
        {
            if( !myIsDatabaseInitialized )
            {
                InitializeDatabase( path );
                myIsDatabaseInitialized = true;
            }
            return new EntitiesContext( path );
        }

        // TODO: this code should be async - and not on-demand but "hidden"
        private void InitializeDatabase( string path )
        {
            SchemaInfo schemaInfo = null;

            if( !File.Exists( path ) )
            {
                SQLiteConnection.CreateFile( path );
                schemaInfo = new SchemaInfo { Id = -1, Version = 0 };
            }

            using( var context = new EntitiesContext( path ) )
            {
                if( schemaInfo == null )
                {
                    schemaInfo = context.SchemaInfos.Single();
                }

                var migrationScript = new DBMigrationScript();
                while( schemaInfo.Version < EntitiesContext.RequiredDatabaseVersion )
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
        }
    }
}
