using System.Data;
using System.IO;
using NUnit.Framework;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers;
using RaynMaker.Modules.Import.Parsers.Text;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Tests.Spec.Extraction
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
