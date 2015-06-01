using System.ComponentModel.Composition;
using System.Linq;

namespace RaynMaker.Entities.Persistancy
{
    [Export]
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

        private void InitializeDatabase( string path )
        {
            using( var context = new EntitiesContext( path ) )
            {
                var schemaInfo = context.SchemaInfos.FirstOrDefault();
                if( schemaInfo == null )
                {
                    schemaInfo = new SchemaInfo { Version = 0 };
                    context.SchemaInfos.Add( schemaInfo );
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
            }
        }
    }
}
