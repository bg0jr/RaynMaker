using Moq;
using System.Linq;
using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.ViewModels;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using System;

namespace RaynMaker.Import.Web.Tests.ViewModels
{
    [TestFixture]
    class BasicDatumProviderTests
    {
        private class DocumentBrowserMock
        {
            public IDocumentBrowser Browser { get; private set; }
            public Navigation Args_Navigation { get; private set; }

            public DocumentBrowserMock()
            {
                var browser = new Mock<IDocumentBrowser>();
                browser.SetupGet( x => x.Document ).Returns( new HtmlDocumentHandle( new Mock<IHtmlDocument>().Object ) );
                browser.Setup( x => x.Navigate( It.IsAny<Navigation>() ) )
                    .Callback<Navigation>( navi => Args_Navigation = navi );

                Browser = browser.Object;
            }
        }

        [Test]
        public void Navigate_IsinMacro_ReplacedCorrectly()
        {
            var browser = new DocumentBrowserMock();
            var provider = new BasicDatumProvider( browser.Browser );

            var navigation = CreateNavigation( DocumentType.Html, "http://www.server.com/search?id=${isin}&paging=off" );

            provider.Navigate( navigation, new Stock { Isin = "AB01010101" } );

            Assert.That( browser.Args_Navigation.Uris.Single().UrlString, Is.EqualTo( "http://www.server.com/search?id=AB01010101&paging=off" ) );
        }

        [Test]
        public void Navigate_WpknMacro_ReplacedCorrectly()
        {
            var browser = new DocumentBrowserMock();
            var provider = new BasicDatumProvider( browser.Browser );

            var navigation = CreateNavigation( DocumentType.Html, "http://www.server.com/search?id=${wpkn}&paging=off" );

            provider.Navigate( navigation, new Stock { Wpkn = "AB0976D" } );

            Assert.That( browser.Args_Navigation.Uris.Single().UrlString, Is.EqualTo( "http://www.server.com/search?id=AB0976D&paging=off" ) );
        }

        [Test]
        public void Navigate_SymbolMacro_ReplacedCorrectly()
        {
            var browser = new DocumentBrowserMock();
            var provider = new BasicDatumProvider( browser.Browser );

            var navigation = CreateNavigation( DocumentType.Html, "http://www.server.com/search?id=${symbol}&paging=off" );

            provider.Navigate( navigation, new Stock { Symbol = "JNJ" } );

            Assert.That( browser.Args_Navigation.Uris.Single().UrlString, Is.EqualTo( "http://www.server.com/search?id=JNJ&paging=off" ) );
        }

        private static Navigation CreateNavigation( DocumentType docType, params string[] urlTemplates )
        {
            bool isRequest = false;
            Func<UriType> GetUriType = () => ( isRequest = !isRequest ) ? UriType.Request : UriType.Response;

            return new Navigation( docType, urlTemplates
                .Select( url => new NavigationUrl( GetUriType(), url ) ) );
        }
    }
}
