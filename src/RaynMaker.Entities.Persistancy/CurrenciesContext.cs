using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;

namespace RaynMaker.Entities.Persistancy
{
    class CurrenciesContext : DbContext, ICurrenciesContext
    {
        public CurrenciesContext( string path )
            : base( GetConnection( path ), true )
        {
            Database.SetInitializer<CurrenciesContext>( null );
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

        public IDbSet<Currency> Currencies { get; set; }
    }
}
