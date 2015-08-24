using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Entities.Datums;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Tests.Fakes
{
    class DatumFactory
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
