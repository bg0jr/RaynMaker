using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class SeriesDescriptorBaseTests
    {
        [DataContract(Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DummyDescriptor")]
        private class DummyDescriptor : SeriesDescriptorBase
        {
            public DummyDescriptor()
            {
            }
        }

        [Test]
        public void Orientation_Set_ValueIsSet()
        {
            var descriptor = new DummyDescriptor();

            descriptor.Orientation = SeriesOrientation.Column;

            Assert.That(descriptor.Orientation, Is.EqualTo(SeriesOrientation.Column));
        }

        [Test]
        public void Orientation_Set_ChangeIsNotified()
        {
            var descriptor = new DummyDescriptor();
            var counter = new PropertyChangedCounter(descriptor);

            descriptor.Orientation = SeriesOrientation.Column;

            Assert.That(counter.GetCount(nameof(descriptor.Orientation)), Is.EqualTo(1));
        }

        [Test]
        public void ValuesLocator_Set_ValueIsSet()
        {
            var descriptor = new DummyDescriptor();

            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 7, SeriesPosition = 4 };

            Assert.That(descriptor.ValuesLocator.HeaderSeriesPosition, Is.EqualTo(7));
        }

        [Test]
        public void ValuesLocator_Set_ChangeIsNotified()
        {
            var descriptor = new DummyDescriptor();
            var counter = new PropertyChangedCounter(descriptor);

            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };

            Assert.That(counter.GetCount(nameof(descriptor.ValuesLocator)), Is.EqualTo(1));
        }

        [Test]
        public void ValueFormat_Set_ValueIsSet()
        {
            var descriptor = new DummyDescriptor();

            descriptor.ValueFormat = new FormatColumn("c1", typeof(double), "0.00");

            Assert.That(descriptor.ValueFormat.Format, Is.EqualTo("0.00"));
        }

        [Test]
        public void ValueFormat_Set_ChangeIsNotified()
        {
            var descriptor = new DummyDescriptor();
            var counter = new PropertyChangedCounter(descriptor);

            descriptor.ValueFormat = new FormatColumn("c1", typeof(double), "0.00");

            Assert.That(counter.GetCount(nameof(descriptor.ValueFormat)), Is.EqualTo(1));
        }

        [Test]
        public void TimesLocator_Set_TimeIsSet()
        {
            var descriptor = new DummyDescriptor();

            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 7, SeriesPosition = 4 };

            Assert.That(descriptor.TimesLocator.HeaderSeriesPosition, Is.EqualTo(7));
        }

        [Test]
        public void TimesLocator_Set_ChangeIsNotified()
        {
            var descriptor = new DummyDescriptor();
            var counter = new PropertyChangedCounter(descriptor);

            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };

            Assert.That(counter.GetCount(nameof(descriptor.TimesLocator)), Is.EqualTo(1));
        }

        [Test]
        public void TimeFormat_Set_TimeIsSet()
        {
            var descriptor = new DummyDescriptor();

            descriptor.TimeFormat = new FormatColumn("c1", typeof(double), "0.00");

            Assert.That(descriptor.TimeFormat.Format, Is.EqualTo("0.00"));
        }

        [Test]
        public void TimeFormat_Set_ChangeIsNotified()
        {
            var descriptor = new DummyDescriptor();
            var counter = new PropertyChangedCounter(descriptor);

            descriptor.TimeFormat = new FormatColumn("c1", typeof(double), "0.00");

            Assert.That(counter.GetCount(nameof(descriptor.TimeFormat)), Is.EqualTo(1));
        }

        [Test]
        public void Excludes_Add_ValueAdded()
        {
            var descriptor = new DummyDescriptor();

            descriptor.Excludes.Add(11);

            Assert.That(descriptor.Excludes, Contains.Item(11));
        }

        [Test]
        public void Excludes_Add_ChangeIsNotified()
        {
            var descriptor = new DummyDescriptor();
            var counter = new CollectionChangedCounter(descriptor.Excludes);

            descriptor.Excludes.Add(11);

            Assert.That(counter.Count, Is.EqualTo(1));
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var descriptor = new DummyDescriptor();

            descriptor.Orientation = SeriesOrientation.Row;

            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.ValueFormat = new FormatColumn("value", typeof(double), "0.00");

            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.TimeFormat = new FormatColumn("time", typeof(DateTime), "G");

            descriptor.Excludes.AddRange(11, 88);

            var clone = FigureDescriptorFactory.Clone(descriptor);

            Assert.That(clone.Orientation, Is.EqualTo(descriptor.Orientation));

            Assert.That(((AbsolutePositionLocator)clone.ValuesLocator).SeriesPosition, Is.EqualTo(4));
            Assert.That(clone.ValueFormat.Name, Is.EqualTo(descriptor.ValueFormat.Name));
            Assert.That(clone.ValueFormat.Type, Is.EqualTo(descriptor.ValueFormat.Type));
            Assert.That(clone.ValueFormat.Format, Is.EqualTo(descriptor.ValueFormat.Format));

            Assert.That(((AbsolutePositionLocator)clone.TimesLocator).SeriesPosition, Is.EqualTo(23));
            Assert.That(clone.TimeFormat.Name, Is.EqualTo(descriptor.TimeFormat.Name));
            Assert.That(clone.TimeFormat.Type, Is.EqualTo(descriptor.TimeFormat.Type));
            Assert.That(clone.TimeFormat.Format, Is.EqualTo(descriptor.TimeFormat.Format));

            Assert.That(clone.Excludes, Is.EquivalentTo(descriptor.Excludes));
        }

        [Test]
        public void Clone_WhenCalled_CollectionsAreMutableAndObservable()
        {
            var descriptor = new DummyDescriptor();

            var clone = FigureDescriptorFactory.Clone(descriptor);

            var counter = new CollectionChangedCounter(clone.Excludes);
            clone.Excludes.Add(1);

            Assert.That(counter.Count, Is.EqualTo(1));
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var descriptor = new DummyDescriptor();
            descriptor.Figure = "Equity";
            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.ValueFormat = new FormatColumn("values", typeof(double), "0.00");

            RecursiveValidator.Validate(descriptor);
        }

        [Test]
        public void Validate_MissingValueLocator_Throws()
        {
            var descriptor = new SeparatorSeriesDescriptor();
            descriptor.ValuesLocator = null;
            descriptor.ValueFormat = new FormatColumn("values", typeof(double), "0.00");

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("The ValuesLocator field is required"));
        }

        [Test]
        public void Validate_MissingValueFormat_Throws()
        {
            var descriptor = new SeparatorSeriesDescriptor();
            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 4 };
            descriptor.ValueFormat = null;

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("The ValueFormat field is required"));
        }

        [Test]
        public void Validate_InvalidValuesLocator_Throws()
        {
            var descriptor = new DummyDescriptor();
            descriptor.Figure = "Equity";
            descriptor.ValuesLocator = new AbsolutePositionLocator();
            descriptor.ValueFormat = new FormatColumn("values", typeof(double), "0.00");

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("HeaderSeriesPosition must be between 0 and " + int.MaxValue));
        }

        [Test]
        public void Validate_InvalidValueFormat_Throws()
        {
            var descriptor = new DummyDescriptor();
            descriptor.Figure = "Equity";
            descriptor.ValuesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.ValueFormat = new FormatColumn();

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("Type field is required"));
        }

        [Test]
        public void Validate_InvalidTimesLocator_Throws()
        {
            var descriptor = new DummyDescriptor();
            descriptor.Figure = "Equity";
            descriptor.TimesLocator = new AbsolutePositionLocator();
            descriptor.TimeFormat = new FormatColumn("Times", typeof(double), "0.00");

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("HeaderSeriesPosition must be between 0 and " + int.MaxValue));
        }

        [Test]
        public void Validate_InvalidTimeFormat_Throws()
        {
            var descriptor = new DummyDescriptor();
            descriptor.Figure = "Equity";
            descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0, SeriesPosition = 23 };
            descriptor.TimeFormat = new FormatColumn();

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(descriptor));
            Assert.That(ex.Message, Does.Contain("Type field is required"));
        }
    }
}
