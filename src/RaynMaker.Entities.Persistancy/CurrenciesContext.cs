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

        // http://stackoverflow.com/questions/5082991/influencing-foreign-key-column-naming-in-ef-code-first-ctp5
        // http://weblogs.asp.net/manavi/associations-in-ef-4-1-code-first-part-6-many-valued-associations
        protected override void OnModelCreating( DbModelBuilder builder )
        {
            builder.Entity<Translation>()
                .HasRequired( t => t.Source )
                .WithMany( c => c.Translations )
                .HasForeignKey( t => t.SourceId )
                .WillCascadeOnDelete(false);

            builder.Entity<Translation>()
                .HasRequired( a => a.Target )
                .WithMany()
                .HasForeignKey( u => u.TargetId )
                .WillCascadeOnDelete( false );
        }
        public IDbSet<Currency> Currencies { get; set; }
    }
}
