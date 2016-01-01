using Moq;
using System.Linq;
using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Web.ViewModels;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using System;
using RaynMaker.Modules.Import.Spec.v2.Locating;
using RaynMaker.Modules.Import.Spec.v2;

namespace RaynMaker.Modules.Import.Web.Tests.ViewModels
{
    [TestFixture]
    class BasicDatumProviderTests
    {
        private class DocumentBrowserMock
        {
            public IDocumentBrowser Browser { get; private set; }
            public DocumentLocator Args_Navigation { get; private set; }

            public DocumentBrowserMock()
            {
                var browser = new Mock<IDocumentBrowser>();
                browser.SetupGet( x => x.Document ).Returns( new Mock<IHtmlDocument>().Object );
                browser.Setup( x => x.Navigate( It.IsAny<DocumentType>(), It.IsAny<DocumentLocator>() ) )
                    .Callback<DocumentType, DocumentLocator>( ( docType, navi ) => Args_Navigation = navi );

                Browser = browser.Object;
            }
        }

        [Test]
        public void Navigate_IsinMacro_ReplacedCorrectly()
        {
            var browser = new DocumentBrowserMock();
            var provider = new BasicDatumProvider( browser.Browser );

            var navigation = CreateNavigation( "http://www.server.com/search?id=${isin}&paging=off" );

            provider.Navigate( DocumentType.Html, navigation, new Stock { Isin = "AB01010101" } );

            Assert.That( browser.Args_Navigation.Fragments.Single().UrlString, Is.EqualTo( "http://www.server.com/search?id=AB01010101&paging=off" ) );
        }

        [Test]
        public void Navigate_WpknMacro_ReplacedCorrectly()
        {
            var browser = new DocumentBrowserMock();
            var provider = new BasicDatumProvider( browser.Browser );

            var navigation = CreateNavigation( "http://www.server.com/search?id=${wpkn}&paging=off" );

            provider.Navigate( DocumentType.Html, navigation, new Stock { Wpkn = "AB0976D" } );

            Assert.That( browser.Args_Navigation.Fragments.Single().UrlString, Is.EqualTo( "http://www.server.com/search?id=AB0976D&paging=off" ) );
        }

        [Test]
        public void Navigate_SymbolMacro_ReplacedCorrectly()
        {
            var browser = new DocumentBrowserMock();
            var provider = new BasicDatumProvider( browser.Browser );

            var navigation = CreateNavigation( "http://www.server.com/search?id=${symbol}&paging=off" );

            provider.Navigate( DocumentType.Html, navigation, new Stock { Symbol = "JNJ" } );

            Assert.That( browser.Args_Navigation.Fragments.Single().UrlString, Is.EqualTo( "http://www.server.com/search?id=JNJ&paging=off" ) );
        }

        private static DocumentLocator CreateNavigation( params string[] urlTemplates )
        {
            bool isRequest = false;

            return new DocumentLocator( urlTemplates
                .Select( url => ( isRequest = !isRequest ) ? ( DocumentLocationFragment )new Request( url ) : ( DocumentLocationFragment )new Response( url ) ) );
        }
    }
}
