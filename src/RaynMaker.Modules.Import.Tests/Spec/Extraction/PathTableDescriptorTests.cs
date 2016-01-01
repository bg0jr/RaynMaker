using System;
using NUnit.Framework;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class PathTableDescriptorTests 
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new PathTableDescriptor( "dummy", "111" );

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Path, Is.EqualTo( "111" ) );
        }
    }
}
