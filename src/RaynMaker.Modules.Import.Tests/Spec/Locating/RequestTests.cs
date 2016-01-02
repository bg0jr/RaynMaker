using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Locating
{
    [TestFixture]
    public class RequestTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var fragment = new Request( "http://test1.org" );

            var clone = FigureDescriptorFactory.Clone( fragment );

            Assert.That( clone.UrlString, Is.EqualTo( "http://test1.org" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var fragment = new Request( "http://www.me.com" );

            RecursiveValidator.Validate( fragment );
        }
    }
}
