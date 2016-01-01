using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using RaynMaker.Entities;

namespace RaynMaker.Data.Views
{
    class ChangeToForegroundConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value is double? )
            {
                var dValue = ( double? )value;
                return dValue == null || dValue.Value == 0 ? Brushes.Black :
                    dValue.Value < 0 ? Brushes.Red : Brushes.Green;
            }

            if( value is double )
            {
                var dValue = ( double )value;
                return dValue == 0 ? Brushes.Black :
                    dValue < 0 ? Brushes.Red : Brushes.Green;
            }

            return null;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
