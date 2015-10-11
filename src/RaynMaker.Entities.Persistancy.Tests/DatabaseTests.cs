using System.IO;
using System.Threading;
using NUnit.Framework;

namespace RaynMaker.Entities.Persistancy.Tests
{
    /// <summary>
    /// Combines all tests together which are relatest to low level Database setup - e.g. cascading delete constraints
    /// </summary>
    [TestFixture]
    class DatabaseTests
    {
        private DatabaseService myDb;

        [SetUp]
        public void SetUp()
        {
            myDb = new DatabaseService( Path.Combine( Path.GetTempPath(), "rym.db" ) );
            myDb.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete( myDb.Location );
            Thread.Sleep( 100 );
        }

        //[Test]
        public void DeleteCompany_WithAllPossibleAssociatedEntities_DeleteCascades()
        {
            var company = new Company { Name = "Dummy" };
            company.References.Add( new Reference { Url = "http://www.not-existent.org/" } );

            var stock = new Stock { Isin = "US123456789" };
            company.Stocks.Add( stock );

            //stock.Prices.Add( new Price() );

            // TODO: add all datums in a generic way

            using( var ctx = ( AssetsContext )myDb.CreateAssetsContext() )
            {
                ctx.Companies.Add( company );

                ctx.SaveChangesSafe();
            }
        }
    }
}
