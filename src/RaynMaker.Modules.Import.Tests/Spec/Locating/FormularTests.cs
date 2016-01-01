using System;
using NUnit.Framework;
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

            var clone = FormatFactory.Clone( form );

            Assert.That( clone.Name, Is.EqualTo( "dummy.f1" ) );
            Assert.That( clone.Parameters[ "p1" ], Is.EqualTo( "v1" ) );
        }
    }
}
