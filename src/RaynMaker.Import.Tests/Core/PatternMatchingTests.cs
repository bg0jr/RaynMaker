using NUnit.Framework;
using RaynMaker.Import.Documents;

namespace RaynMaker.Import.Tests.Core
{
    [TestFixture]
    public class PatternMatchingTests
    {
        [Test]
        public void MatchSimpleString()
        {
            string pattern = @"hiho{(\d+)}bla";
            string value = PatternMatching.MatchEmbeddedRegex( pattern, "hiho1234bla" );

            Assert.AreEqual( "1234", value );
        }

        [Test]
        public void MatchWithEscape()
        {
            string pattern = @"http://bla.de?hiho.x={(\d+)}";
            string value = PatternMatching.MatchEmbeddedRegex( pattern, "http://bla.de?hiho.x=438" );

            Assert.AreEqual( "438", value );
        }

        [Test]
        public void NoEmbeddedRegEx()
        {
            string pattern = @"http://bla.de?hiho.x=";
            string value = PatternMatching.MatchEmbeddedRegex( pattern, "http://bla.de?hiho.x=" );

            Assert.AreEqual( null, value );
        }

        [Test]
        public void NotMatched()
        {
            string pattern = @"http://bla.de?hiho.x={(\d+)}";
            string value = PatternMatching.MatchEmbeddedRegex( pattern, "http://bla.de?hiho.x=bfd" );

            Assert.AreEqual( null, value );
        }
    }
}
