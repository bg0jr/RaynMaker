using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Entities.Persistancy
{
    class AssetsContext : DbContext, IAssetsContext
    {
        public AssetsContext( string path )
            : base( GetConnection( path ), true )
        {
            Database.SetInitializer<AssetsContext>( null );

            ( ( IObjectContextAdapter )this ).ObjectContext
                             .ObjectMaterialized += ObjectContext_OnObjectMaterialized;

            //this.Database.Log = stmt => Debug.WriteLine( "SQL: " + stmt );
        }

        private void ObjectContext_OnObjectMaterialized( object sender, ObjectMaterializedEventArgs e )
        {
            var entityTimestampBase = e.Entity as EntityTimestampBase;
            if( entityTimestampBase != null )
            {
                entityTimestampBase.IsMaterialized = true;
            }
        }

        private static DbConnection GetConnection( string path )
        {
            var builder = new SQLiteConnectionStringBuilder
                {
                    DataSource = path,
                    ForeignKeys = true,
                    // http://stackoverflow.com/questions/27279177/how-does-the-sqlite-entity-framework-6-provider-handle-guids
                    BinaryGUID = true
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
                .WillCascadeOnDelete( true );

            builder.Entity<Translation>()
                .HasRequired( a => a.Target )
                .WithMany()
                .HasForeignKey( u => u.TargetId )
                .WillCascadeOnDelete( true );
        }

        public IDbSet<Currency> Currencies { get; set; }

        public IDbSet<Company> Companies { get; set; }

        public IDbSet<Stock> Stocks { get; set; }

        public DbSet<SchemaInfo> SchemaInfos { get; set; }
    }
}
