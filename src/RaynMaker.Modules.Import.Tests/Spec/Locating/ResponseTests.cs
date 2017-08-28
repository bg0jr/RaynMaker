using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Tests.Spec.Locating
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var fragment = new Response( "http://test1.org" );

            var clone = FigureDescriptorFactory.Clone( fragment );

            Assert.That( clone.UrlString, Is.EqualTo( "http://test1.org" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var fragment = new Response( "http://www.me.com" );

            RecursiveValidator.Validate( fragment );
        }
    }
}
