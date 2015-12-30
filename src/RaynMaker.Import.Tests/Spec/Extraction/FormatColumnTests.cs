using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class FormatColumnTests : TestBase
    {
        [Test]
        public void Ctor_WhenCalled_NameIsSet()
        {
            var col = new FormatColumn( "test" );

            Assert.That( col.Name, Is.EqualTo( "test" ) );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var col = new FormatColumn("c1");

            var clone = FormatFactory.Clone( col );

            Assert.That( clone.Name, Is.EqualTo( "c1" ) );
        }
    }
}
