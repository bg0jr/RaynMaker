using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.Tests.Spec.Locating
{
    [TestFixture]
    public class LocatingFragmentTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var navi = new DocumentLocationFragment( UriType.Request, "http://test1.org" );

            var clone = FormatFactory.Clone( navi );

            Assert.That( clone.UrlType, Is.EqualTo( UriType.Request ) );
            Assert.That( clone.UrlString, Is.EqualTo( "http://test1.org" ) );
        }

        [Test]
        public void Clone_WithFormular_AllMembersAreCloned()
        {
            var navi = new DocumentLocationFragment( new Formular( "dummy.form" ) );

            var clone = FormatFactory.Clone( navi );

            Assert.That( clone.UrlType, Is.EqualTo( UriType.SubmitFormular ) );
            Assert.That( clone.Formular.Name, Is.EqualTo( "dummy.form" ) );
        }
    }
}
