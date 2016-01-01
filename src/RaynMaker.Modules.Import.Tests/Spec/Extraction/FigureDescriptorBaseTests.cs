using System.Runtime.Serialization;
using NUnit.Framework;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Tests;

namespace RaynMaker.Modules.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class FigureDescriptorBaseTests
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyFormat" )]
        private class DummyFormat : FigureDescriptorBase
        {
            public DummyFormat()
                : base( "dummy" )
            {
            }
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new DummyFormat();
            format.Figure = "blue";
            format.InMillions = true;

            var clone = FormatFactory.Clone( format );

            Assert.That( clone.Figure, Is.EqualTo( format.Figure ) );
            Assert.That( clone.InMillions, Is.EqualTo( format.InMillions ) );
        }
    }
}
