using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Plainion;

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
            var dbFile = Path.GetTempFileName() + ".rymdb";

            Contract.Requires( !File.Exists( dbFile ), "DB file already/still exists" );

            myDb = new DatabaseService( dbFile );
            myDb.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            myDb.Shutdown();

            // TODO: no way found until now to get rid of the file locks from SQlite :(
            // -> delete all files matching our file name pattern to at least do some partial cleanup

            foreach( var file in Directory.GetFiles( Path.GetTempPath(), "*.rymdb", SearchOption.TopDirectoryOnly ) )
            {
                try
                {
                    File.Delete( file );
                }
                catch( IOException ) { }
            }

            Thread.Sleep( 100 );
        }

        [Test]
        public void DeleteCompany_WithAllPossibleAssociatedEntities_DeleteCascades()
        {
            // setup entities object model
            var company = new Company { Name = "Dummy" };
            {
                company.References.Add( new Reference { Url = "http://www.not-existent.org/" } );

                var stock = new Stock { Isin = "US123456789" };
                stock.Company = company;
                company.Stocks.Add( stock );

                var currency = new Currency { Name = "Dollar" };

                // add fake datums for all known types
                foreach( var datumType in Dynamics.AllDatums )
                {
                    AddFakeDatum( stock, datumType, currency );
                }
            }

            // save entity object model to DB and validate that DB is updated correctly
            using( var ctx = ( AssetsContext )myDb.CreateAssetsContext() )
            {
                ctx.Companies.Add( company );

                ctx.SaveChangesSafe();

                Assert.That( ctx.Database.SqlQuery<object>( "SELECT Id FROM Companies" ), Is.Not.Empty );
                Assert.That( ctx.Database.SqlQuery<object>( "SELECT Id FROM 'References'" ), Is.Not.Empty );

                foreach( var datumType in Dynamics.AllDatums )
                {
                    Assert.That( ctx.Database.SqlQuery<object>( "SELECT Id FROM " + Dynamics.GetTableName( datumType ) ), Is.Not.Empty,
                        "Table '{0}' expected to be NOT empty", Dynamics.GetTableName( datumType ) );
                }
            }

            // delete entity object model from DB and validate that DB is updated correctly
            using( var ctx = ( AssetsContext )myDb.CreateAssetsContext() )
            {
                company = ctx.Companies.Single();

                ctx.Companies.Remove( company );

                ctx.SaveChangesSafe();

                // validate that deletion applied correctly to DB
                Assert.That( ctx.Database.SqlQuery<object>( "SELECT Id FROM Companies" ), Is.Empty );
                Assert.That( ctx.Database.SqlQuery<object>( "SELECT Id FROM 'References'" ), Is.Empty );

                foreach( var datumType in Dynamics.AllDatums )
                {
                    Assert.That( ctx.Database.SqlQuery<object>( "SELECT Id FROM " + Dynamics.GetTableName( datumType ) ), Is.Empty,
                        "Table '{0}' expected to be empty", Dynamics.GetTableName( datumType ) );
                }
            }
        }

        // TODO: add for currencies and 

        private void AddFakeDatum( Stock stock, Type datumType, Currency currency )
        {
            var datum = Dynamics.CreateDatum( stock, datumType, new DayPeriod( DateTime.UtcNow ), currency );

            var requiredProperties = datumType.GetProperties()
                // will be set by EF when saving datum
                .Where( p => p.Name != "Id" )
                // set by "CreateDatum" already
                .Where( p => p.Name != "Period" && p.Name != "RawPeriod" && p.Name != "Currency" && p.Name != "Company" && p.Name != "Stock" )
                // updated automatically
                .Where( p => p.Name != "Timestamp" )
                .Where( p => p.GetCustomAttributes( typeof( RequiredAttribute ), true ).Any() );

            foreach( var prop in requiredProperties )
            {
                if( prop.PropertyType == typeof( double ) || prop.PropertyType == typeof( double? ) )
                {
                    prop.SetValue( datum, 42d );
                }
                else if( prop.PropertyType == typeof( string ) )
                {
                    prop.SetValue( datum, "dummy" );
                }
                else
                {
                    throw new NotSupportedException( "Don't know how to set required property: " + prop.Name );
                }
            }

            // add to relationship
            var datums = ( IList )Dynamics.GetRelationship( stock, datumType );
            datums.Add( datum );
        }
    }
}
