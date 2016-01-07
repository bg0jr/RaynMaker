using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;


namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    [RequiresSTA]
    class DefiningFigureDescriptors : TestBase
    {
        [Test]
        public void DefineCell()
        {
            var doc = LoadDocument<IHtmlDocument>( "Html", "ariva.prices.DE0007664039.html" );

            var dataSource = new DataSource();
            dataSource.Vendor = "Ariva";
            dataSource.Name = "Prices";
            dataSource.Quality = 1;

            var descriptor = new PathCellDescriptor();
            descriptor.Figure = "Price";
            descriptor.Path = @"/BODY[0]/DIV[0]/DIV[1]/DIV[6]/DIV[1]/DIV[0]/DIV[0]/TABLE[0]/TBODY[0]";
            descriptor.Column = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "Letzter" };
            descriptor.Row = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "Frankfurt" };
            descriptor.ValueFormat = new FormatColumn( "value", typeof( double ), "00,00" ) { ExtractionPattern = new Regex( @"([0-9,\.]+)" ) };
            descriptor.Currency = "EUR";

            //var viewModel = new Path
        }
    }
}
