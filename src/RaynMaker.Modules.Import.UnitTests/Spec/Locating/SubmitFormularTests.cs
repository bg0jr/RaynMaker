using System;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Locating
{
    [TestFixture]
    public class SubmitFormularTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var fragment = new SubmitFormular( "http://test1.org", new Formular( "dummy.form" ) );

            var clone = FigureDescriptorFactory.Clone( fragment );

            Assert.That( clone.UrlString, Is.EqualTo( "http://test1.org" ) );
            Assert.That( clone.Formular.Name, Is.EqualTo( "dummy.form" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var fragment = new SubmitFormular( "http://test1.org", new Formular( "dummy.form" ) );

            RecursiveValidator.Validate( fragment );
        }

        [Test]
        public void Ctor_UrlStringValidFormularIsNull_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>( () => new SubmitFormular( "http://test1.org", null ) );
            Assert.That( ex.Message, Does.Contain( "Value cannot be null." + Environment.NewLine + "Parameter name: form" ) );
        }

        [Test]
        public void Ctor_UrlValidFormularIsNull_Throws()
        {
            var ex = Assert.Throws<ArgumentNullException>( () => new SubmitFormular( new Uri( "http://test1.org" ), null ) );
            Assert.That( ex.Message, Does.Contain( "Value cannot be null." + Environment.NewLine + "Parameter name: form" ) );
        }
    }
}
