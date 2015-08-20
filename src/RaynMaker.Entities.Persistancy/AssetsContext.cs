using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;

namespace RaynMaker.Entities.Persistancy
{
    class AssetsContext : DbContext, IAssetsContext
    {
        public AssetsContext( string path )
            : base( GetConnection( path ), true )
        {
            Database.SetInitializer<AssetsContext>( null );
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

        public IDbSet<Company> Companies { get; set; }

        public IDbSet<Stock> Stocks { get; set; }

        public DbSet<SchemaInfo> SchemaInfos { get; set; }
    }
}
