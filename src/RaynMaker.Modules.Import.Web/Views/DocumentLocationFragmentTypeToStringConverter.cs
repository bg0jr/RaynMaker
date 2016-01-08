using System;
using System.Windows.Data;

namespace RaynMaker.Modules.Import.Web.Views
{
    class DocumentLocationFragmentTypeToStringConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return value.GetType().Name;
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
