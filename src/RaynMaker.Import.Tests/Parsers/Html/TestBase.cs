using System.IO;
using NUnit.Framework;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.Tests.Html
{
    public class TestBase : RaynMaker.Import.Tests.TestBase
    {
        private IDocumentBrowser myBrowser = null;

        [TestFixtureSetUp]
        public virtual void FixtureSetUp()
        {
            myBrowser = DocumentProcessorsFactory.CreateBrowser();
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

            var navi = new DocumentLocator(  new DocumentLocationFragment( UriType.Request, file ) );
            myBrowser.Navigate( DocumentType.Html, navi );
            var doc = ( HtmlDocumentHandle )myBrowser.Document;

            return doc.Content;
        }
    }
}
