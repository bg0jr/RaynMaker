using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.SDK;

namespace RaynMaker.Modules.Import.UnitTests.Spec.Extraction
{
    [TestFixture]
    public class ValueFormatTests
    {
        [Test]
        public void Type_Set_ValueIsSet()
        {
            var format = new ValueFormat();

            format.Type = typeof(string);

            Assert.That(format.Type, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void Type_Set_ChangeIsNotified()
        {
            var format = new ValueFormat();
            var counter = new PropertyChangedCounter(format);

            format.Type = typeof(string);

            Assert.That(counter.GetCount(nameof(format.Type)), Is.EqualTo(1));
        }

        [Test]
        public void Format_Set_ValueIsSet()
        {
            var format = new ValueFormat();

            format.Format = "#0.00";

            Assert.That(format.Format, Is.EqualTo("#0.00"));
        }

        [Test]
        public void Format_Set_ChangeIsNotified()
        {
            var format = new ValueFormat();
            var counter = new PropertyChangedCounter(format);

            format.Format = "#0.00";

            Assert.That(counter.GetCount(nameof(format.Format)), Is.EqualTo(1));
        }

        [Test]
        public void ExtractionPattern_Set_ValueIsSet()
        {
            var format = new ValueFormat();

            format.ExtractionPattern = new Regex(@"(\d+)");

            Assert.That(format.ExtractionPattern.ToString(), Is.EqualTo(@"(\d+)"));
        }

        [Test]
        public void ExtractionPattern_Set_ChangeIsNotified()
        {
            var format = new ValueFormat();
            var counter = new PropertyChangedCounter(format);

            format.ExtractionPattern = new Regex(@"(\d+)");

            Assert.That(counter.GetCount(nameof(format.ExtractionPattern)), Is.EqualTo(1));
        }

        [Test]
        public void ConvertString()
        {
            var format = new ValueFormat(typeof(string));
            object value = format.Convert("hiho");

            Assert.AreEqual("hiho", value);
        }

        [Test]
        public void ConvertRegExString()
        {
            var format = new ValueFormat(typeof(int), "000000") { ExtractionPattern = new Regex(@"WKN: ([\d]+)") };
            object value = format.Convert("WKN: 850206");

            Assert.AreEqual(850206, (int)value);
        }

        [Test]
        public void ConvertCurrency()
        {
            var format = new ValueFormat(typeof(double), "000000.00") { ExtractionPattern = new Regex(@"([\d.]+)\s*€") };

            double value = (double)format.Convert("2.5€");
            Assert.AreEqual(2.5, value, 0.000001d);

            value = (double)format.Convert("0.25 €");
            Assert.AreEqual(0.25, value, 0.000001d);
        }

        [Test]
        public void ConvertDouble()
        {
            var format = new ValueFormat(typeof(double), "00.00");
            double value = (double)format.Convert("2.5");

            Assert.AreEqual(2.5, value);
        }

        [Test]
        public void ConvertDoubleWithoutFormat()
        {
            var format = new ValueFormat(typeof(double));
            object value = format.Convert("2.5");

            Assert.IsFalse(value is double);
        }

        [Test]
        public void ConvertDateTime()
        {
            var format = new ValueFormat(typeof(DateTime), "dd.MM.yyyy");
            DateTime value = (DateTime)format.Convert("12.12.2000");

            Assert.AreEqual(2000, value.Year);
            Assert.AreEqual(12, value.Month);
            Assert.AreEqual(12, value.Day);
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var format = new ValueFormat(typeof(double), "##0.00")
            {
                ExtractionPattern = new Regex(@"(\d+)$"),
                InMillions = true
            };

            var clone = FigureDescriptorFactory.Clone(format);

            Assert.That(clone.Type, Is.EqualTo(typeof(double)));
            Assert.That(clone.Format, Is.EqualTo("##0.00"));
            Assert.That(clone.ExtractionPattern.ToString(), Is.EqualTo(@"(\d+)$"));
            Assert.That(clone.InMillions, Is.EqualTo(format.InMillions));
        }

        [Test]
        public void Validate_IsValid_DoesNotThrows()
        {
            var format = new ValueFormat(typeof(string));
            format.InMillions = true;

            RecursiveValidator.Validate(format);
        }

        [Test]
        public void Validate_TypeIsNull_Throws()
        {
            var format = new ValueFormat();

            var ex = Assert.Throws<ValidationException>(() => RecursiveValidator.Validate(format));
            Assert.That(ex.Message, Does.Contain("Type field is required"));
        }

        [Test]
        public void InMillions_Set_ValueIsSet()
        {
            var format = new ValueFormat();

            format.InMillions = true;

            Assert.That(format.InMillions, Is.True);
        }

        [Test]
        public void InMillions_Set_ChangeIsNotified()
        {
            var format = new ValueFormat();
            var counter = new PropertyChangedCounter(format);

            format.InMillions = true;

            Assert.That(counter.GetCount(nameof(format.InMillions)), Is.EqualTo(1));
        }

        [Test]
        public void Convert_InMillions_ValueMultipliedByOneMillion()
        {
            var format = new ValueFormat(typeof(double), "00.00");
            format.InMillions = true;

            double value = (double)format.Convert("2.5");

            Assert.That(value, Is.EqualTo(2.5 * 1000000));
        }
    }
}
