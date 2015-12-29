using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Spec
{
    [TestFixture]
    public class NavigationTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var navi = new Navigation( DocumentType.Html,
                new NavigationUrl( UriType.Request, "http://test1.org" ),
                new NavigationUrl( UriType.Response, "http://test2.org" ) );

            var clone = FormatFactory.Clone( navi );

            Assert.That( clone.DocumentType, Is.EqualTo( DocumentType.Html ) );
            Assert.That( clone.UrisHashCode, Is.EqualTo( navi.UrisHashCode ) );

            Assert.That( clone.Uris[ 0 ].UrlString, Is.EqualTo( "http://test1.org" ) );
            Assert.That( clone.Uris[ 1 ].UrlString, Is.EqualTo( "http://test2.org" ) );
        }
    }
}
