using System;
using System.Linq;
using NUnit.Framework;
using RaynMaker.Modules.Import.Documents.AgilityPack;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Tests.Html
{
    abstract class HtmlTableTestsBase
    {
        protected abstract string GetHtml();

        [Test]
        public void Rows_WhenCalled_AllTRElementsReturned()
        {
            var doc = HtmlDocumentLoader.LoadHtml( GetHtml() );
            var table = HtmlTable.GetByPath( doc, HtmlPath.Parse( "/BODY[0]/TABLE[0]" ) );

            Assert.That( table.Rows.Count, Is.EqualTo( 4 ) );
        }

        [Test]
        public void GetRow_WithIndex_ReturnsCorrectTDs()
        {
            var doc = HtmlDocumentLoader.LoadHtml( GetHtml() );
            var table = HtmlTable.GetByPath( doc, HtmlPath.Parse( "/BODY[0]/TABLE[0]" ) );

            var row = table.GetRow( 1 );

            Assert.That( row[ 0 ].Parent.Id, Is.EqualTo( "row1" ) );
        }

        [Test]
        public void GetRow_WithCell_ReturnsCorrectTDs()
        {
            var doc = HtmlDocumentLoader.LoadHtml( GetHtml() );
            var table = HtmlTable.GetByPath( doc, HtmlPath.Parse( "/BODY[0]/TABLE[0]" ) );

            var row = table.GetRow( doc.GetElementById( "c12" ) );

            Assert.That( row[ 0 ].Parent.Id, Is.EqualTo( "row1" ) );
        }

        [Test]
        public void RowIndexOf_WithCell_ReturnsCorrectIndex()
        {
            var doc = HtmlDocumentLoader.LoadHtml( GetHtml() );
            var table = HtmlTable.GetByPath( doc, HtmlPath.Parse( "/BODY[0]/TABLE[0]" ) );

            var idx = table.RowIndexOf( doc.GetElementById( "c12" ) );

            Assert.That( idx, Is.EqualTo( 1 ) );
        }

        [Test]
        public void GetColumn_WithIndex_ReturnsCorrectTDs()
        {
            var doc = HtmlDocumentLoader.LoadHtml( GetHtml() );
            var table = HtmlTable.GetByPath( doc, HtmlPath.Parse( "/BODY[0]/TABLE[0]" ) );

            var column = table.GetColumn( 2 );

            Assert.That( column.Select( td => td.Id ), Is.EquivalentTo( new[] { "c02", "c12", "c22", "c32" } ) );
        }

        [Test]
        public void GetColumn_WithCell_ReturnsCorrectTDs()
        {
            var doc = HtmlDocumentLoader.LoadHtml( GetHtml() );
            var table = HtmlTable.GetByPath( doc, HtmlPath.Parse( "/BODY[0]/TABLE[0]" ) );

            var column = table.GetColumn( doc.GetElementById( "c12" ) );

            Assert.That( column.Select( td => td.Id ), Is.EquivalentTo( new[] { "c02", "c12", "c22", "c32" } ) );
        }

        [Test]
        public void ColumnIndexOf_WithCell_ReturnsCorrectIndex()
        {
            var doc = HtmlDocumentLoader.LoadHtml( GetHtml() );
            var table = HtmlTable.GetByPath( doc, HtmlPath.Parse( "/BODY[0]/TABLE[0]" ) );

            var idx = table.ColumnIndexOf( doc.GetElementById( "c12" ) );

            Assert.That( idx, Is.EqualTo( 2 ) );
        }

        [Test]
        public void GetCell_WithRowAndColumn_ReturnsCorrectCell()
        {
            var doc = HtmlDocumentLoader.LoadHtml( GetHtml() );
            var table = HtmlTable.GetByPath( doc, HtmlPath.Parse( "/BODY[0]/TABLE[0]" ) );

            var element = table.GetCell( 1, 2 );

            Assert.That( element.Id, Is.EqualTo( "c12" ) );
        }
    }
}
