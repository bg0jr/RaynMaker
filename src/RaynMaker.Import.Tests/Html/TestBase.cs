using System.IO;
using NUnit.Framework;
using RaynMaker.Import.Core;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Html
{
    public class TestBase : RaynMaker.Import.Tests.TestBase
    {
        private IDocumentBrowser myBrowser = null;

        [TestFixtureSetUp]
        public virtual void FixtureSetUp()
        {
            myBrowser = DocumentBrowserFactory.Create();
        }

        [TestFixtureTearDown]
        public virtual void FixtureTearDown()
        {
            myBrowser = null;
        }

        protected IHtmlDocument LoadDocument( string name )
        {
            string file = Path.Combine( TestDataRoot,  "Html" );
            file = Path.Combine( file, name );

            var navi = new Navigation( DocumentType.Html, new NavigatorUrl( UriType.Request, file ) );
            var doc = (HtmlDocumentHandle)myBrowser.GetDocument( navi );

            return doc.Content;
        }
    }
}
