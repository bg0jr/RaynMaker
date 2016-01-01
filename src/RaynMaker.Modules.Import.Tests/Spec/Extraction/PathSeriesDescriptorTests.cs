using NUnit.Framework;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class PathSeriesDescriptorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new PathSeriesDescriptor( "dummy" );
            format.Path = "123";

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Path, Is.EqualTo( "123" ) );
        }
    }
}
