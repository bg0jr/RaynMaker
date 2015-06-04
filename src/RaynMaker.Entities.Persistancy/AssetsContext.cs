using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;

namespace RaynMaker.Entities.Persistancy
{
    class AssetsContext : DbContext, IAssetsContext
    {
        static AssetsContext()
        {
            DbConfiguration.SetConfiguration( new SQLiteConfiguration() );
        }

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

        public DbSet<Company> Companies { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        IEnumerable<Company> IAssetsContext.Companies { get { return Companies; } }

        IEnumerable<Stock> IAssetsContext.Stocks { get { return Stocks; } }

        public DbSet<SchemaInfo> SchemaInfos { get; set; }
    }
}
