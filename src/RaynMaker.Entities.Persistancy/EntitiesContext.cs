using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;

namespace RaynMaker.Entities.Persistancy
{
    class EntitiesContext : DbContext, IEntityContext
    {
        internal static int RequiredDatabaseVersion = 1;

        static EntitiesContext()
        {
            DbConfiguration.SetConfiguration( new SQLiteConfiguration() );
        }

        public EntitiesContext( string path )
            : base( GetConnection( path ), true )
        {
            Database.SetInitializer<EntitiesContext>( null );
        }

        private static DbConnection GetConnection( string path )
        {
            var builder = new SQLiteConnectionStringBuilder
                {
                    DataSource = path,
                    ForeignKeys = true,
                };

            return new SQLiteConnection( builder.ConnectionString );
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        IEnumerable<Company> IEntityContext.Companies { get { return Companies; } }

        IEnumerable<Stock> IEntityContext.Stocks { get { return Stocks; } }

        public DbSet<SchemaInfo> SchemaInfos { get; set; }
    }
}
