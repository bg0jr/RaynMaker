using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Tests.Spec
{
    [TestFixture]
    public class PathCellFormatTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new PathCellFormat( "dummy");
            format.Currency = "Euro";

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Currency, Is.EqualTo( "Euro" ) );
        }
    }
}
