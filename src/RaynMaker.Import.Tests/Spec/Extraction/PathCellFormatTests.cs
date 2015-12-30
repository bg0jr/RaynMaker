using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class PathCellFormatTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new PathCellDescriptor( "dummy");
            format.Currency = "Euro";

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Currency, Is.EqualTo( "Euro" ) );
        }
    }
}
