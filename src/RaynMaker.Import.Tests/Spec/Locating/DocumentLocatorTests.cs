using System;
using System.IO;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.Tests.Spec.Locating
{
    [TestFixture]
    public class DocumentLocatorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var navi = new DocumentLocator( 
                new DocumentLocationFragment( UriType.Request, "http://test1.org" ),
                new DocumentLocationFragment( UriType.Response, "http://test2.org" ) );

            var clone = FormatFactory.Clone( navi );

            Assert.That( clone.UrisHashCode, Is.EqualTo( navi.UrisHashCode ) );

            Assert.That( clone.Uris[ 0 ].UrlString, Is.EqualTo( "http://test1.org" ) );
            Assert.That( clone.Uris[ 1 ].UrlString, Is.EqualTo( "http://test2.org" ) );
        }
    }
}
