using NUnit.Framework;
using RaynMaker.Modules.Import.Documents.AgilityPack;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.UnitTests.Html
{
    [TestFixture]
    public class HtmlDocumentExtensionsTests
    {
        [Test]
        public void GetElementByPath_PathPointsToRoot_RootReturned()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body></body></html>" );

            var path = HtmlPath.Parse( "/" );

            var element = doc.GetElementByPath( path );

            Assert.That( element, Is.EqualTo( doc.Body.GetRoot() ) );
        }

        [Test]
        public void GetElementByPath_SimplePath_ElementFound()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div id='xx'/></body></html>" );

            var path = HtmlPath.Parse( "/BODY[0]/div[0]" );

            var element = doc.GetElementByPath( path );

            Assert.That( element, Is.EqualTo( doc.GetElementById( "xx" ) ) );
        }

        [Test]
        public void GetElementByPath_NoElementWithSuchPath_ReturnsNull()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div id='xx'/></body></html>" );

            var path = HtmlPath.Parse( "/html[0]/BODY[0]/P[0]" );

            var element = doc.GetElementByPath( path );

            Assert.That( element, Is.Null );
        }

        [Test]
        public void GetElementByPath_MultipleElementsWithSameTagName_ElementFound()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div/><div/><div><P/><P/><p id='xx'/></div></body></html>" );

            var path = HtmlPath.Parse( "/BODY[0]/div[2]/P[2]" );

            var element = doc.GetElementByPath( path );

            Assert.That( element, Is.EqualTo( doc.GetElementById( "xx" ) ) );
        }
    }
}
