﻿using System;
using System.Globalization;
using System.Windows.Data;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Analysis.AnalysisSpec
{
    class PeriodChartConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var yearPeriod = value as YearPeriod;
            if( yearPeriod != null )
            {
                return yearPeriod.Year.ToString().Substring( 2 );
            }

            var dayPeriod = value as DayPeriod;
            if( dayPeriod != null )
            {
                return dayPeriod.Day.ToShortDateString();
            }

            return value;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
