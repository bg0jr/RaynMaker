using System;
using System.Globalization;
using System.Windows.Data;

namespace RaynMaker.Analysis.AnalysisSpec
{
    class InMillionsConverter : IValueConverter
    {
        public bool InMillions { get; set; }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return InMillions ? ( ( double )value ) / 1000000 : value;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
