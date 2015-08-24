using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using RaynMaker.Blade.Entities;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Views
{
    public class CurrencyVisibilityConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value is ICurrencyDatum )
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
