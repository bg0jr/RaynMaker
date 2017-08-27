using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class FigureDescriptorBaseTests
    {
        [DataContract(Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyDesciptor")]
        private class DummyDesciptor : FigureDescriptorBase
        {
            public DummyDesciptor()
            {
            }
        }

        [Test]
        public void Figure_Set_ValueIsSet()
        {
            var descriptor = new DummyDesciptor();

            descriptor.Figure = "EPS";

            Assert.That(descriptor.Figure, Is.EqualTo("EPS"));
        }

        [Test]
        public void Figure_Set_ChangeIsNotified()
        {
            var descriptor = new DummyDesciptor();
            var counter = new PropertyChangedCounter(descriptor);

            descriptor.Figure = "EPS";

            Assert.That(counter.GetCount(nameof(descriptor.Figure)), Is.EqualTo(1));
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new DummyDesciptor();
            descriptor.Figure = "blue";

            var clone = FigureDescriptorFactory.Clone(descriptor);

            Assert.That(clone.Figure, Is.EqualTo(descriptor.Figure));
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new DummyDesciptor();
            descriptor.Figure = "blue";

            RecursiveValidator.Validate(descriptor);
        }

        [Test]
        public void Validate_InvalidFigure_Throws([Values(null, "")]string figure)
        {
            var descriptor = new DummyDesciptor();
            descriptor.Figure = figure;

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Is.StringContaining("The Figure field is required"));
        }
    }
}
