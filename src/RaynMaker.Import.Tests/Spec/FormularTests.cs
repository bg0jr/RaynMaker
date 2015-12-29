using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Spec
{
    [TestFixture]
    public class FormularTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var form = new Formular( "dummy.f1" );
            form.Parameters[ "p1" ] = "v1";

            var clone = FormatFactory.Clone( form );

            Assert.That( clone.Name, Is.EqualTo( "dummy.f1" ) );
            Assert.That( clone.Parameters[ "p1" ], Is.EqualTo( "v1" ) );
        }
    }
}
