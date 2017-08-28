using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class CsvDescriptorTests
    {
        [Test]
        public void Separator_Set_ValueIsSet()
        {
            var descriptor = new CsvDescriptor();

            descriptor.Separator = ";";

            Assert.That(descriptor.Separator, Is.EqualTo(";"));
        }

        [Test]
        public void Separator_Set_ChangeIsNotified()
        {
            var descriptor = new CsvDescriptor();
            var counter = new PropertyChangedCounter(descriptor);

            descriptor.Separator = ";";

            Assert.That(counter.GetCount(nameof(descriptor.Separator)), Is.EqualTo(1));
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new CsvDescriptor();
            descriptor.Figure = "dummy";
            descriptor.Separator = ";";
            descriptor.Columns.Add(new FormatColumn("c1", typeof(double), "0.00"));

            var clone = FigureDescriptorFactory.Clone(descriptor);

            Assert.That(clone.Separator, Is.EqualTo(";"));

            Assert.That(clone.Columns[0].Name, Is.EqualTo("c1"));
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new CsvDescriptor();
            descriptor.Figure = "dummy";
            descriptor.Separator = ";";
            descriptor.Columns.Add(new FormatColumn("c1", typeof(double), "0.00"));

            RecursiveValidator.Validate(descriptor);
        }

        [Test]
        public void Validate_SeparatorNull_Throws()
        {
            var descriptor = new CsvDescriptor();
            descriptor.Figure = "dummy";
            descriptor.Separator = null;
            descriptor.Columns.Add(new FormatColumn("c1", typeof(double), "0.00"));

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("Separator field is required"));
        }

        [Test]
        public void Validate_ColumnsEmpty_Throws()
        {
            var descriptor = new CsvDescriptor();
            descriptor.Figure = "dummy";
            descriptor.Separator = ";";
            descriptor.Columns.Clear();

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("Columns must not be empty"));
        }
    }
}
