﻿using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.ScenarioTests
{
    [TestFixture]
    [RequiresSTA]
    public class ReadingFromHtmlDocumentScenarios : TestBase
    {
        [Test]
        public void GetSingleValue()
        {
            var doc = LoadDocument<IHtmlDocument>( "ariva.overview.US0138171014.html" );

            var descriptor = new PathSingleValueDescriptor();
            descriptor.Path = @"/BODY[0]/DIV[4]/DIV[0]/DIV[3]/DIV[0]";
            descriptor.ValueFormat = new ValueFormat( typeof( int ), "00000000" ) { ExtractionPattern = new Regex( @"WKN: (\d+)" ) };

            var parser = DocumentProcessorsFactory.CreateParser( doc, descriptor );
            var table = parser.ExtractTable();

            Assert.AreEqual( 1, table.Rows.Count );

            Assert.AreEqual( 850206, table.Rows[ 0 ][ 0 ] );
        }

        [Test]
        public void GetSeries()
        {
            var doc = LoadDocument<IHtmlDocument>( "ariva.html" );

            var descriptor = new PathSeriesDescriptor();
            descriptor.Figure = "EPS";
            descriptor.Path = @"/BODY[0]/DIV[5]/DIV[0]/DIV[1]/TABLE[7]/TBODY[0]";
            descriptor.Orientation = SeriesOrientation.Row;
            descriptor.ValuesLocator = new StringContainsLocator { HeaderSeriesPosition = 0, Pattern = "verwässertes Ergebnis pro Aktie" };
            descriptor.ValueFormat = new FormatColumn( "value", typeof( float ), "00,00" );
            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 1 };
            descriptor.TimeFormat = new FormatColumn( "year", typeof( int ), "00000000" );
            descriptor.Excludes.Add( 0 );

            var parser = DocumentProcessorsFactory.CreateParser( doc, descriptor );
            var table = parser.ExtractTable();

            Assert.AreEqual( 6, table.Rows.Count );

            Assert.AreEqual( 2.78f, table.Rows[ 0 ][ 0 ] );
            Assert.AreEqual( 3.00f, table.Rows[ 1 ][ 0 ] );
            Assert.AreEqual( 2.89f, table.Rows[ 2 ][ 0 ] );
            Assert.AreEqual( 3.30f, table.Rows[ 3 ][ 0 ] );
            Assert.AreEqual( 3.33f, table.Rows[ 4 ][ 0 ] );
            Assert.AreEqual( 4.38f, table.Rows[ 5 ][ 0 ] );

            Assert.AreEqual( 2001, table.Rows[ 0 ][ 1 ] );
            Assert.AreEqual( 2002, table.Rows[ 1 ][ 1 ] );
            Assert.AreEqual( 2003, table.Rows[ 2 ][ 1 ] );
            Assert.AreEqual( 2004, table.Rows[ 3 ][ 1 ] );
            Assert.AreEqual( 2005, table.Rows[ 4 ][ 1 ] );
            Assert.AreEqual( 2006, table.Rows[ 5 ][ 1 ] );
        }

        [Test]
        public void GetCell()
        {
        }

        [Test]
        public void GetTable()
        {
        }
    }
}
