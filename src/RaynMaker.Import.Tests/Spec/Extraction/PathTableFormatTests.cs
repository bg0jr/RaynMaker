using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class PathTableFormatTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new PathTableExtractionDescriptor( "dummy", "111" );

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Path, Is.EqualTo( "111" ) );
        }
    }
}
