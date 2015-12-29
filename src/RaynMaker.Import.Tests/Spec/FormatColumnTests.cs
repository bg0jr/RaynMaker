using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Spec
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
