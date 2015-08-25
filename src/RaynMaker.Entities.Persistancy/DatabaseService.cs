using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Plainion;

namespace RaynMaker.Entities.Persistancy
{
    public class DatabaseService : IContextFactory
    {
        private static bool myIsInitialized;

        static DatabaseService()
        {
            DbConfiguration.SetConfiguration( new SQLiteConfiguration() );
        }

        public DatabaseService( string location )
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

                var migrationScript = new DatabaseMigrations();
                while( schemaInfo.Version < DatabaseMigrations.RequiredDatabaseVersion )
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

        public IAnalysisContext CreateAnalysisContext()
        {
            Contract.Invariant( myIsInitialized, "Initialized() not yet called" );

            return new AnalysisContext( Location );
        }

        public ICurrenciesContext CreateCurrenciesContext()
        {
            Contract.Invariant( myIsInitialized, "Initialized() not yet called" );

            return new CurrenciesContext( Location );
        }
    }
}
