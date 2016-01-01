using NUnit.Framework;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Locating
{
    [TestFixture]
    public class SubmitFormularTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var navi = new SubmitFormular( "http://test1.org", new Formular( "dummy.form" ) );

            var clone = FormatFactory.Clone( navi );

            Assert.That( clone.UrlString, Is.EqualTo( "http://test1.org" ) );
            Assert.That( clone.Formular.Name, Is.EqualTo( "dummy.form" ) );
        }
    }
}
