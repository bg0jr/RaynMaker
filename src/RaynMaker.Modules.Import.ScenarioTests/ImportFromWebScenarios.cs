using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Moq;
using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web;
using RaynMaker.Modules.Import.Web.Services;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    [RequiresSTA]
    public class ImportFromWebScenarios : TestBase
    {
        private Mock<ILutService> myLutService;
        private Mock<IProjectHost> myProjectHost;

        [SetUp]
        public void SetUp()
        {
            myLutService = new Mock<ILutService> { DefaultValue = DefaultValue.Mock };
            Mock.Get( myLutService.Object.CurrenciesLut ).SetupGet( x => x.Currencies ).Returns( new ObservableCollection<Entities.Currency>() );
            myLutService.Object.CurrenciesLut.Currencies.Add( new Entities.Currency { Symbol = "EUR" } );

            myProjectHost = new Mock<IProjectHost> { DefaultValue = DefaultValue.Mock };
            myProjectHost.Setup( x => x.Project.StorageRoot ).Returns( Path.Combine( TestDataRoot, "DataSources", "ImportFromWebScenarios" ) );
        }

        [Test]
        public void GetSeries()
        {
            var stock = new Stock { Isin = "DE0005190003" };
            stock.Company = new Company { Name = "BMW" };
            stock.Company.Stocks.Add( stock );

            var dataProvider = new WebDatumProvider( new StorageService( myProjectHost.Object ), myLutService.Object );

            var request = new DataProviderRequest( stock, typeof( Dividend ), new YearPeriod( 2001 ), new YearPeriod( 2004 ) )
            {
                WithPreview = false,
                ThrowOnError = true
            };

            var series = new List<IDatum>();
            dataProvider.Fetch( request, series );

            Assert.That( series.Count, Is.EqualTo( 4 ) );

            foreach( var dividend in series.Cast<Dividend>() )
            {
                Assert.That( dividend.Company.Stocks.First().Isin, Is.EqualTo( "DE0005190003" ) );
                Assert.That( dividend.Period, Is.InstanceOf<YearPeriod>() );
                Assert.That( dividend.Source, Is.StringContaining( "ariva" ).IgnoreCase.And.StringContaining( "fundamentals" ).IgnoreCase );
                Assert.That( dividend.Timestamp.Date, Is.EqualTo( DateTime.Today ) );
                Assert.That( dividend.Currency, Is.Null );
            }

            Assert.That( series[ 0 ].Period, Is.EqualTo( new YearPeriod( 2001 ) ) );
            Assert.That( series[ 0 ].Value, Is.EqualTo( 350000000d ) );
            Assert.That( series[ 1 ].Period, Is.EqualTo( new YearPeriod( 2002 ) ) );
            Assert.That( series[ 1 ].Value, Is.EqualTo( 351000000d ) );
            Assert.That( series[ 2 ].Period, Is.EqualTo( new YearPeriod( 2003 ) ) );
            Assert.That( series[ 2 ].Value, Is.EqualTo( 392000000d ) );
            Assert.That( series[ 3 ].Period, Is.EqualTo( new YearPeriod( 2004 ) ) );
            Assert.That( series[ 3 ].Value, Is.EqualTo( 419000000d ) );
        }

        [Test]
        public void GetCell()
        {
            var stock = new Stock { Isin = "DE0007664039" };
            stock.Company = new Company { Name = "Volkswagen" };
            stock.Company.Stocks.Add( stock );

            var dataProvider = new WebDatumProvider( new StorageService( myProjectHost.Object ), myLutService.Object );

            var request = new DataProviderRequest( stock, typeof( Price ), new DayPeriod( DateTime.MinValue ), new DayPeriod( DateTime.MaxValue ) )
            {
                WithPreview = false,
                ThrowOnError = true
            };

            var series = new List<IDatum>();
            dataProvider.Fetch( request, series );

            Assert.That( series.Count, Is.EqualTo( 1 ) );

            var price = ( Price )series.Single();

            Assert.That( price.Stock.Isin, Is.EqualTo( "DE0007664039" ) );
            Assert.That( ( ( DayPeriod )price.Period ).Day.Date, Is.EqualTo( DateTime.Today ) );
            Assert.That( price.Source, Is.StringContaining( "ariva" ).IgnoreCase.And.StringContaining( "price" ).IgnoreCase );
            Assert.That( price.Timestamp.Date, Is.EqualTo( DateTime.Today ) );
            Assert.That( price.Value, Is.EqualTo( 134.356d ) );
            Assert.That( price.Currency.Symbol, Is.EqualTo( "EUR" ) );
        }
    }
}
