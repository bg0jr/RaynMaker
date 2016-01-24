using System;
using System.ComponentModel;
using RaynMaker.Entities;
using RaynMaker.Entities.Figures;

namespace RaynMaker.Entities.UnitTests.Fakes
{
    public class FigureFactory
    {
        public static IFigure New( int year, double value )
        {
            return new FakeFigure
            {
                Period = new YearPeriod( year ),
                Value = value,
                Source = "Dummy",
            };
        }

        public static IFigure New( int year, double value, Currency currency )
        {
            return new FakeCurrencyFigure
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
