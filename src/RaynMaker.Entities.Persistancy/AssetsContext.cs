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

            ( ( IObjectContextAdapter )this ).ObjectContext.ObjectMaterialized += ObjectContext_OnObjectMaterialized;

            //this.Database.Log = stmt => Debug.WriteLine( "SQL: " + stmt );
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

        private void ObjectContext_OnObjectMaterialized( object sender, ObjectMaterializedEventArgs e )
        {
            var entityTimestampBase = e.Entity as EntityTimestampBase;
            if( entityTimestampBase != null )
            {
                entityTimestampBase.IsMaterialized = true;
            }
        }

        protected override void Dispose( bool disposing )
        {
            try
            {
                ( ( IObjectContextAdapter )this ).ObjectContext.ObjectMaterialized -= ObjectContext_OnObjectMaterialized;
            }
            finally
            {
                base.Dispose( disposing );
            }
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
                .HasRequired( t => t.Target )
                .WithMany()
                .HasForeignKey( t => t.TargetId )
                .WillCascadeOnDelete( true );

            builder.Entity<Company>()
                .HasMany( c => c.Tags )
                .WithMany()
                .Map( m =>
                  {
                      m.MapLeftKey( "Company_Id" );
                      m.MapRightKey( "Tag_Id" );
                      m.ToTable( "CompanyTags" );
                  } );
        }

        public IDbSet<Currency> Currencies { get; set; }

        public IDbSet<Company> Companies { get; set; }

        public IDbSet<Stock> Stocks { get; set; }

        public DbSet<SchemaInfo> SchemaInfos { get; set; }
       
        public IDbSet<Tag> Tags { get; set; }
    }
}
