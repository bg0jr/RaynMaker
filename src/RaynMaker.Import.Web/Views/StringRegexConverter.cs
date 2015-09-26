using System;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace RaynMaker.Import.Web.Views
{
    class StringRegexConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if( value == null )
            {
                return null;
            }

            return ( ( Regex )value ).ToString();
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if( value == null )
            {
                return null;
            }

            return new Regex( value.ToString() );
        }
    }
}
