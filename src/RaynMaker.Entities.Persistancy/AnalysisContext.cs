using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;

namespace RaynMaker.Entities.Persistancy
{
    class AnalysisContext : DbContext, IAnalysisContext
    {
        public AnalysisContext( string path )
            : base( GetConnection( path ), true )
        {
            Database.SetInitializer<AnalysisContext>( null );
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

        public IDbSet<AnalysisTemplate> AnalysisTemplates { get; set; }
    }
}
