using NUnit.Framework;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Tests.Html
{
    [TestFixture]
    public class HtmlPathElementTests
    {
        [Test]
        public void Ctor_WhenCalled_TagNameConvertedToUppperAndPositonSet()
        {
            var e = new HtmlPathElement( "Tr", 3 );

            Assert.That( e.TagName, Is.EqualTo( "TR" ) );
            Assert.That( e.Position, Is.EqualTo( 3 ) );
        }

        [Test]
        public void ToString_WhenCalled_ReturnsStringInParsableForm()
        {
            var e = new HtmlPathElement( "Tr", 3 );

            Assert.That( e.ToString(), Is.EqualTo( "TR[3]" ) );

            var clone = HtmlPathElement.Parse( e.ToString() );

            Assert.That( clone.TagName, Is.EqualTo( "TR" ) );
            Assert.That( clone.Position, Is.EqualTo( 3 ) );
        }

        [Test]
        public void IsTable_ForTable_ReturnsTrue()
        {
            var e = new HtmlPathElement( "Table", 3 );

            Assert.That( e.IsTable, Is.True );
        }

        [Test]
        public void IsTable_ForTBody_ReturnsFalse()
        {
            var e = new HtmlPathElement( "tbody", 3 );

            Assert.That( e.IsTable, Is.False );
        }

        [Test]
        public void IsTable_ForTHead_ReturnsFalse()
        {
            var e = new HtmlPathElement( "thead", 3 );

            Assert.That( e.IsTable, Is.False );
        }

        [Test]
        public void IsTableOrTBody_ForTR_ReturnsFalse()
        {
            var e = new HtmlPathElement( "TR", 3 );

            Assert.That( e.IsTable, Is.False );
        }

        [Test]
        public void IsTableCell_ForTD_ReturnsTrue()
        {
            var e = new HtmlPathElement( "TD", 3 );

            Assert.That( e.IsTableCell, Is.True );
        }

        [Test]
        public void IsTableCell_ForTBody_ReturnsTrue()
        {
            var e = new HtmlPathElement( "TH", 3 );

            Assert.That( e.IsTableCell, Is.True );
        }

        [Test]
        public void IsTableCell_ForTR_ReturnsFalse()
        {
            var e = new HtmlPathElement( "TR", 3 );

            Assert.That( e.IsTableCell, Is.False );
        }

        [Test]
        public void Parse_WhenCalled_TagNameAndPositionExtracted()
        {
            var e = HtmlPathElement.Parse( "table[2]" );

            Assert.AreEqual( "TABLE", e.TagName );
            Assert.AreEqual( 2, e.Position );
        }
    }
}
