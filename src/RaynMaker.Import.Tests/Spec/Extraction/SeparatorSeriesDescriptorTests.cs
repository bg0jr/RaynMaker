using System.Data;
using System.IO;
using NUnit.Framework;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers;
using RaynMaker.Import.Parsers.Text;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class SeparatorSeriesDescriptorTests
    {
        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new SeparatorSeriesDescriptor( "dummy" );
            format.Separator = "#";

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Separator, Is.EqualTo( "#" ) );
        }
    }
}
