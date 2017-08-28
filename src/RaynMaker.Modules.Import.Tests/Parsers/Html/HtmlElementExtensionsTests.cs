using NUnit.Framework;
using RaynMaker.Modules.Import.Documents.AgilityPack;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Tests.Html
{
    [TestFixture]
    public class HtmlElementExtensionsTests
    {
        [Test]
        public void GetRoot_WithRoot_RootReturned()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body></body></html>" );

            Assert.That( doc.Body.GetRoot(), Is.EqualTo( doc.Body.Parent ) );
        }

        [Test]
        public void GetRoot_SomeChild_RootReturned()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div id='xx'/></body></html>" );

            var root = doc.GetElementById( "xx" ).GetRoot();

            Assert.That( root, Is.EqualTo( doc.Body.Parent ) );
        }

        [Test]
        public void GetPath_SimplePath_PathFound()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div id='xx'/></body></html>" );

            var path = doc.GetElementById( "xx" ).GetPath();

            Assert.That( path.ToString(), Is.EqualTo( "/BODY[0]/DIV[0]" ) );
        }

        [Test]
        public void GetPath_MultipleElementsWithSameTagName_CorrectPathWithCorrectChildrenPosFound()
        {
            var doc = HtmlDocumentLoader.LoadHtml( "<html><body><div/><div/><div><P/><P/><p id='xx'/></div></body></html>" );

            var path = doc.GetElementById( "xx" ).GetPath();

            Assert.That( path.ToString(), Is.EqualTo( "/BODY[0]/DIV[2]/P[2]" ) );
        }
    }
}
