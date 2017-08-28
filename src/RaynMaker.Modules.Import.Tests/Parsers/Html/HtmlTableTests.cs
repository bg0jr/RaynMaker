using System;
using NUnit.Framework;
using RaynMaker.Modules.Import.Documents.AgilityPack;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Tests.Html
{
    [TestFixture]
    class HtmlTableTests
    {
        [Test]
        public void Ctor_NotATableElement_Throws()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div id='xx'/></body></html>" );
            var element = doc.GetElementById( "xx" );

            var ex = Assert.Throws<ArgumentException>( () => new HtmlTable( element ) );
            Assert.That( ex.Message, Does.Contain( "not a html table element" ) );
        }
    }
}
