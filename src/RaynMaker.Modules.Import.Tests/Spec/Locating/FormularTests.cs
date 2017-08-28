using System;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Tests.Spec.Locating
{
    [TestFixture]
    public class FormularTests 
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var form = new Formular( "dummy.f1" );
            form.Parameters[ "p1" ] = "v1";

            var clone = FigureDescriptorFactory.Clone( form );

            Assert.That( clone.Name, Is.EqualTo( "dummy.f1" ) );
            Assert.That( clone.Parameters[ "p1" ], Is.EqualTo( "v1" ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var form = new Formular( "dummy.f1" );

            RecursiveValidator.Validate( form );
        }

        [Test]
        public void Ctor_UrlStringIsNullOrEmpty_Throws( [Values( null, "" )] string name )
        {
            var ex = Assert.Throws<ArgumentNullException>( () => new Formular( name ) );
            Assert.That( ex.Message, Does.Contain( "string must not null or empty: name" ) );
        }
    }
}
