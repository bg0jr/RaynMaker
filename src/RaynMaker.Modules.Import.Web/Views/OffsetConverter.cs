using System;
using System.Windows.Data;

namespace RaynMaker.Modules.Import.Web.Views
{
    class OffsetConverter : IValueConverter
    {
        public int Offset { get; set; }

        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return ( ( int )value ) + Offset;
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return ( ( int )value ) - Offset;
        }
    }
}
