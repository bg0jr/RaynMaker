using System.Collections.Generic;
using NUnit.Framework;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.UnitTests.Documents
{
    [TestFixture]
    class AbstractMacroResolverTests
    {
        class Dummy : AbstractMacroResolver
        {
            public Dummy()
            {
                Macros = new Dictionary<string, string>();
            }

            public IDictionary<string, string> Macros { get; private set; }

            protected override string GetMacroValue( string macroId )
            {
                return Macros.ContainsKey( macroId ) ? Macros[ macroId ] : null;
            }
        }

        [Test]
        public void Resolve_SingleMacro_ReplacedCorrectly()
        {
            var resolver = new Dummy();
            resolver.Macros[ "m1" ] = "abc";

            var result = resolver.Resolve( new Request( "http://www.server.com/search?id=${m1}&paging=off" ) );

            Assert.That( result.UrlString, Is.EqualTo( "http://www.server.com/search?id=abc&paging=off" ) );
        }

        [Test]
        public void Resolve_MultipleMacros_AllMacrosReplaced()
        {
            var resolver = new Dummy();
            resolver.Macros[ "m1" ] = "abc";
            resolver.Macros[ "m2" ] = "xyz";

            var result = resolver.Resolve( new Request( "${m2}/search?id=${m1}&paging=off" ) );

            Assert.That( result.UrlString, Is.EqualTo( "xyz/search?id=abc&paging=off" ) );
        }

    }
}
