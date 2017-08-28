using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using RaynMaker.Entities.Persistancy;

namespace RaynMaker.Entities.Specs
{
    [TestFixture]
    class CascadingDeleteTests : DatabaseTestsBase
    {
        static object[] AllFigures = Dynamics.AllFigures.ToArray();

        private Currency myCurrency;

        [SetUp]
        public void SetUp()
        {
            myCurrency = new Currency { Name = "U.S. Dollar", Symbol = "USD" };
        }

        [Test]
        public void DeleteCompany_WithoutAnyAssociatedData_GetsDeleted()
        {
            var company = new Company { Name = "Dummy" };

            SaveAndVerify( company, "Companies", "Id", c => c.Id );

            DeleteAndVerify( company, "Companies", "Id", c => c.Id );
        }

        [Test]
        public void DeleteCompany_WithStock_DeleteCascades()
        {
            var company = CreateFakeCompanyWithStock();

            SaveAndVerify( company, "Stocks", "Company_Id", c => c.Id );

            DeleteAndVerify( company, "Stocks", "Company_Id", c => c.Id );
        }

        [Test]
        public void DeleteCompany_WithReferences_DeleteCascades()
        {
            var company = CreateFakeCompanyWithStock();

            company.References.Add( new Reference { Url = "http://www.not-existent.org/" } );

            SaveAndVerify( company, "'References'", "Company_Id", c => c.Id );

            DeleteAndVerify( company, "'References'", "Company_Id", c => c.Id );
        }

        [Test, TestCaseSource( "AllFigures" )]
        public void DeleteCompany_WithFigure_DeleteCascades( Type figureType )
        {
            var company = CreateFakeCompanyWithStock();

            AddFakeFigure( company.Stocks.Single(), figureType, myCurrency );

            var tableName = DatabaseConventions.GetTableName( figureType );

            string ownerIdColumn;
            Func<Company, long> GetId;

            var property = typeof( Company ).GetProperty( tableName );
            if( property != null )
            {
                ownerIdColumn = "Company_Id";
                GetId = c => c.Id;
            }
            else
            {
                property = typeof( Stock ).GetProperty( tableName );
                if( property != null )
                {
                    ownerIdColumn = "Stock_Id";
                    GetId = c => c.Stocks.Single().Id;
                }
                else
                {
                    throw new NotSupportedException( "Don't know how to detect owner" );
                }
            }

            SaveAndVerify( company, tableName, ownerIdColumn, c => c.Id );

            DeleteAndVerify( company, tableName, ownerIdColumn, c => c.Id );
        }

        private Company CreateFakeCompanyWithStock()
        {
            var company = new Company { Name = "Dummy" };

            var stock = new Stock { Isin = "US123456789" };
            stock.Company = company;
            company.Stocks.Add( stock );

            return company;
        }

        private void SaveAndVerify( Company company, string tableToVerify, string idColumn, Func<Company, long> GetId )
        {
            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                ctx.Companies.Add( company );

                ctx.SaveChangesSafe();

                DBAssert.RowExists( ctx.Database, tableToVerify, idColumn, GetId( company ) );
            }
        }

        private void DeleteAndVerify( Company company, string tableToVerify, string idColumn, Func<Company, long> GetId )
        {
            using( var ctx = ( AssetsContext )Db.CreateAssetsContext() )
            {
                company = ctx.Companies.Single();
                var id = GetId( company );

                ctx.Companies.Remove( company );

                ctx.SaveChangesSafe();

                DBAssert.RowNotExists( ctx.Database, tableToVerify, idColumn, id );
            }
        }

        private void AddFakeFigure( Stock stock, Type figureType, Currency currency )
        {
            var figure = Dynamics.CreateFigure( stock, figureType, new DayPeriod( DateTime.UtcNow ), currency );

            var requiredProperties = figureType.GetProperties()
                // will be set by EF when saving figure
                .Where( p => p.Name != "Id" )
                // set by "CreateFigure" already
                .Where( p => p.Name != "Period" && p.Name != "RawPeriod" && p.Name != "Currency" && p.Name != "Company" && p.Name != "Stock" )
                // updated automatically
                .Where( p => p.Name != "Timestamp" )
                .Where( p => p.GetCustomAttributes( typeof( RequiredAttribute ), true ).Any() );

            foreach( var prop in requiredProperties )
            {
                if( prop.PropertyType == typeof( double ) || prop.PropertyType == typeof( double? ) )
                {
                    prop.SetValue( figure, 42d );
                }
                else if( prop.PropertyType == typeof( string ) )
                {
                    prop.SetValue( figure, "dummy" );
                }
                else
                {
                    throw new NotSupportedException( "Don't know how to set required property: " + prop.Name );
                }
            }

            // add to relationship
            var figures = ( IList )Dynamics.GetRelationship( stock, figureType );
            figures.Add( figure );
        }
    }
}
