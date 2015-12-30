using System;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class PathSingleValueFormatTests : TestBase
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new PathSingleValueFormat( "dummy" );
            format.Path = "111";
            format.ValueFormat = new ValueFormat( typeof( int ), "0.xx" );

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Path, Is.EqualTo( "111" ) );
            Assert.That( clone.ValueFormat.Format, Is.EqualTo( "0.xx" ) );
        }
    }
}
