using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.UnitTests;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class FigureDescriptorBaseTests
    {
        [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyDesciptor" )]
        private class DummyDesciptor : FigureDescriptorBase
        {
            public DummyDesciptor()
                : base( "dummy" )
            {
            }
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new DummyDesciptor();
            descriptor.Figure = "blue";
            descriptor.InMillions = true;

            var clone = FigureDescriptorFactory.Clone( descriptor );

            Assert.That( clone.Figure, Is.EqualTo( descriptor.Figure ) );
            Assert.That( clone.InMillions, Is.EqualTo( descriptor.InMillions ) );
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new DummyDesciptor();
            descriptor.Figure = "blue";
            descriptor.InMillions = true;

            RecursiveValidator.Validate( descriptor );
        }

        [Test]
        public void Validate_InvalidFigure_Throws( [Values( null, "" )]string figure )
        {
            var descriptor = new DummyDesciptor();
            descriptor.Figure = figure;
            descriptor.InMillions = true;

            var ex = Assert.Throws<ValidationException>( () => RecursiveValidator.Validate( descriptor ) );
            Assert.That( ex.Message, Is.StringContaining( "The Figure field is required" ) );
        }
    }
}
