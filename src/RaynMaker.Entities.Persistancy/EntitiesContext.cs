using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace RaynMaker.Entities.Persistancy
{
    class EntitiesContext : DbContext, IEntityContext
    {
        internal static int RequiredDatabaseVersion = 1;

        public EntitiesContext( string path )
            : base( new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder
                {
                    DataSource = path,
                    ForeignKeys = true,
                }.ConnectionString
            }, true )
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        IEnumerable<Company> IEntityContext.Companies { get { return Companies; } }

        IEnumerable<Stock> IEntityContext.Stocks { get { return Stocks; } }

        internal DbSet<SchemaInfo> SchemaInfos { get; set; }
    }
}
