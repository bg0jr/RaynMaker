using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class PathSeriesDescriptorTests
    {
        [Test]
        public void Path_Set_ValueIsSet()
        {
            var descriptor = new PathSeriesDescriptor();

            descriptor.Path = "123";

            Assert.That(descriptor.Path, Is.EqualTo("123"));
        }

        [Test]
        public void Path_Set_ChangeIsNotified()
        {
            var descriptor = new PathSeriesDescriptor();
            var counter = new PropertyChangedCounter(descriptor);

            descriptor.Path = "123";

            Assert.That(counter.GetCount(nameof(descriptor.Path)), Is.EqualTo(1));
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new PathSeriesDescriptor();
            descriptor.Path = "123";

            var clone = FigureDescriptorFactory.Clone(descriptor);

            Assert.That(clone.Path, Is.EqualTo("123"));
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new PathSeriesDescriptor();
            descriptor.Figure = "Equity";
            descriptor.Path = "123";
            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.ValueFormat = new FormatColumn("values", typeof(double), "0.00");

            RecursiveValidator.Validate(descriptor);
        }

        [Test]
        public void Validate_InvalidPath_Throws([Values(null, "")]string path)
        {
            var descriptor = new PathSeriesDescriptor();
            descriptor.Figure = "Equity";
            descriptor.Path = path;
            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.ValueFormat = new FormatColumn("values", typeof(double), "0.00");

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("The Path field is required"));
        }
    }
}
