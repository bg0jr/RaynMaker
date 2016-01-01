using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace RaynMaker.Modules.Browser.Views
{
    class CollectionToCommaSeparatedStringConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value == null ) return null;

            return string.Join( ",", ( IEnumerable<object> )value );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
