using NUnit.Framework;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Spec.v2.Locating;
using RaynMaker.Modules.Import.Web.ViewModels;

namespace RaynMaker.Modules.Import.Web.Tests.ViewModels
{
    [TestFixture]
    class StockMacroResolverTests
    {
        [Test]
        public void Resolve_IsinMacro_ReplacedCorrectly()
        {
            var resolver = new StockMacroResolver( new Stock { Isin = "AB01010101" } );

            var result = resolver.Resolve( new Request( "http://www.server.com/search?id=${isin}&paging=off" ) );

            Assert.That( result.UrlString, Is.EqualTo( "http://www.server.com/search?id=AB01010101&paging=off" ) );
        }

        [Test]
        public void Resolve_WpknMacro_ReplacedCorrectly()
        {
            var resolver = new StockMacroResolver( new Stock { Wpkn = "AB0976D" } );

            var result = resolver.Resolve( new Request( "http://www.server.com/search?id=${wpkn}&paging=off" ) );

            Assert.That( result.UrlString, Is.EqualTo( "http://www.server.com/search?id=AB0976D&paging=off" ) );
        }

        [Test]
        public void Resolve_SymbolMacro_ReplacedCorrectly()
        {
            var resolver = new StockMacroResolver( new Stock { Symbol = "JNJ" } );

            var result = resolver.Resolve( new Request( "http://www.server.com/search?id=${symbol}&paging=off" ) );

            Assert.That( result.UrlString, Is.EqualTo( "http://www.server.com/search?id=JNJ&paging=off" ) );
        }
    }
}
