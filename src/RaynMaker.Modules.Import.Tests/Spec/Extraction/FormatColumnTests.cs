using System;
using NUnit.Framework;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class FormatColumnTests 
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
