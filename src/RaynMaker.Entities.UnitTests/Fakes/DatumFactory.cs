using System;
using System.ComponentModel;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Entities.UnitTests.Fakes
{
    public class DatumFactory
    {
        public static IDatum New( int year, double value )
        {
            return new FakeDatum
            {
                Period = new YearPeriod( year ),
                Value = value,
                Source = "Dummy",
            };
        }

        public static IDatum New( int year, double value, Currency currency )
        {
            return new FakeCurrencyDatum
            {
                Period = new YearPeriod( year ),
                Value = value,
                Currency = currency,
                Source = "Dummy",
            };
        }

        public static Price NewPrice( string day, double price, Currency currency )
        {
            var converter = new DateTimeConverter();
            return new Price
            {
                Period = new DayPeriod( ( DateTime )converter.ConvertFrom( day ) ),
                Value = price,
                Currency = currency,
                Source = "Dummy",
            };
        }
    }
}
