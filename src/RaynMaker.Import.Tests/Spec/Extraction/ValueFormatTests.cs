using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Tests.Spec.Extraction
{
    [TestFixture]
    public class ValueFormatTests
    {
        [Test]
        public void EqualsAndHashCode()
        {
            var format = new ValueFormat( typeof( int ), "000000", new Regex( @"WKN: ([\d]+)" ) );
            var same = new ValueFormat( typeof( int ), "000000", new Regex( @"WKN: ([\d]+)" ) );

            Assert.IsTrue( format.Equals( same ) );
            Assert.IsTrue( format.Equals( ( object )same ) );
            Assert.AreEqual( format.GetHashCode(), same.GetHashCode() );
        }

        [Test]
        public void EqualsAndHashCodeFailure()
        {
            var format = new ValueFormat( typeof( int ), "000000", new Regex( @"WKN: ([\d]+)" ) );
            var other = new ValueFormat( typeof( int ), "000000" );

            Assert.IsFalse( format.Equals( other ) );
            Assert.IsFalse( format.Equals( ( object )other ) );
            Assert.AreNotEqual( format.GetHashCode(), other.GetHashCode() );
        }

        [Test]
        public void ConvertString()
        {
            var format = new ValueFormat();
            object value = format.Convert( "hiho" );

            Assert.AreEqual( "hiho", value );
        }

        [Test]
        public void ConvertRegExString()
        {
            var format = new ValueFormat( typeof( int ), "000000", new Regex( @"WKN: ([\d]+)" ) );
            object value = format.Convert( "WKN: 850206" );

            Assert.AreEqual( 850206, ( int )value );
        }

        [Test]
        public void ConvertCurrency()
        {
            var format = new ValueFormat( typeof( double ), "000000.00", new Regex( @"([\d.]+)\s*€" ) );

            double value = ( double )format.Convert( "2.5€" );
            Assert.AreEqual( 2.5, value, 0.000001d );

            value = ( double )format.Convert( "0.25 €" );
            Assert.AreEqual( 0.25, value, 0.000001d );
        }

        [Test]
        public void ConvertDouble()
        {
            var format = new ValueFormat( typeof( double ), "00.00" );
            double value = ( double )format.Convert( "2.5" );

            Assert.AreEqual( 2.5, value );
        }

        [Test]
        public void ConvertdoubleWithoutFormat()
        {
            var format = new ValueFormat( typeof( double ) );
            object value = format.Convert( "2.5" );

            Assert.IsFalse( value is double );
        }

        [Test]
        public void ConvertDateTime()
        {
            var format = new ValueFormat( typeof( DateTime ), "dd.MM.yyyy" );
            DateTime value = ( DateTime )format.Convert( "12.12.2000" );

            Assert.AreEqual( 2000, value.Year );
            Assert.AreEqual( 12, value.Month );
            Assert.AreEqual( 12, value.Day );
        }

        [Test]
        public void Clone_WhenCalled_AllMembersAreCloned()
        {
            var col = new ValueFormat( typeof( double ), "##0.00", new Regex( @"(\d+)$" ) );

            var clone = FormatFactory.Clone( col );

            Assert.That( clone.Type, Is.EqualTo( typeof( double ) ) );
            Assert.That( clone.Format, Is.EqualTo( "##0.00" ) );
            Assert.That( clone.ExtractionPattern.ToString(), Is.EqualTo( @"(\d+)$" ) );
        }
    }
}
