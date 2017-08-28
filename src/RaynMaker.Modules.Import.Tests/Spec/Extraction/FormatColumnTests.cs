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
    public class FormatColumnTests
    {
        [Test]
        public void Name_Set_ValueIsSet()
        {
            var col = new FormatColumn();

            col.Name = "c1";

            Assert.That(col.Name, Is.EqualTo("c1"));
        }

        [Test]
        public void Name_Set_ChangeIsNotified()
        {
            var col = new FormatColumn();
            var counter = new PropertyChangedCounter(col);

            col.Name = "c1";

            Assert.That(counter.GetCount(nameof(col.Name)), Is.EqualTo(1));
        }

        [Test]
        public void Ctor_WhenCalled_NameIsSet()
        {
            var col = new FormatColumn("test", typeof(string));

            Assert.That(col.Name, Is.EqualTo("test"));
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var col = new FormatColumn("c1", typeof(string));

            var clone = FigureDescriptorFactory.Clone(col);

            Assert.That(clone.Name, Is.EqualTo("c1"));
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var col = new FormatColumn("c1", typeof(string));

            RecursiveValidator.Validate(col);
        }

        [Test]
        public void Validate_InvalidColumnName_Throws([Values(null, "")]string columnName)
        {
            var col = new FormatColumn(columnName, typeof(string));

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(col));
            Assert.That(ex.Message, Does.Contain("Name field is required"));
        }
    }
}
