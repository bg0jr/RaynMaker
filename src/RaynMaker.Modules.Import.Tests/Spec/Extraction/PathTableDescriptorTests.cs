using System;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class PathTableDescriptorTests
    {
        [Test]
        public void Path_Set_ValueIsSet()
        {
            var descriptor = new PathTableDescriptor();

            descriptor.Path = "123";

            Assert.That(descriptor.Path, Is.EqualTo("123"));
        }

        [Test]
        public void Path_Set_ChangeIsNotified()
        {
            var descriptor = new PathTableDescriptor();
            var counter = new PropertyChangedCounter(descriptor);

            descriptor.Path = "123";

            Assert.That(counter.GetCount(nameof(descriptor.Path)), Is.EqualTo(1));
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new PathTableDescriptor();
            descriptor.Columns.Add(new FormatColumn("c1", typeof(double)));

            var clone = FigureDescriptorFactory.Clone(descriptor);

            Assert.That(clone.Columns[0].Name, Is.EqualTo("c1"));
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new PathTableDescriptor();
            descriptor.Figure = "Prices";
            descriptor.Path = "123";
            descriptor.Columns.Add(new FormatColumn("c1", typeof(double)));

            RecursiveValidator.Validate(descriptor);
        }

        [Test]
        public void Validate_InvalidPath_Throws([Values(null, "")]string path)
        {
            var descriptor = new PathTableDescriptor();
            descriptor.Figure = "Prices";
            descriptor.Path = path;
            descriptor.Columns.Add(new FormatColumn("c1", typeof(double)));

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("The Path field is required"));
        }
    }
}
