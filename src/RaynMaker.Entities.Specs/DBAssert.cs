using System.Data.Entity;
using NUnit.Framework;

namespace RaynMaker.Entities.Specs
{
    class DBAssert
    {
        public static void RowExists( Database db, string table, string idColumn, long id )
        {
            Assert.That( db.SqlQuery<object>( string.Format( "SELECT * FROM {0} WHERE {1} = {2}", table, idColumn, id ) ), Is.Not.Empty,
                "No data found in table '{0}' with {1} = {2}", table, idColumn, id );
        }

        public static void RowNotExists( Database db, string table, string idColumn, long id )
        {
            Assert.That( db.SqlQuery<object>( string.Format( "SELECT * FROM {0} WHERE {1} = {2}", table, idColumn, id ) ), Is.Empty,
                "Unexpected data found in table '{0}' with {1} = {2}", table, idColumn, id );
        }
    }
}
