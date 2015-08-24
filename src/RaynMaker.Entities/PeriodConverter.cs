using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace RaynMaker.Entities
{
    public sealed class PeriodConverter : TypeConverter, IValueConverter
    {
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
        {
            return sourceType == typeof( string ) || base.CanConvertFrom( context, sourceType );
        }

        public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value )
        {
            if( value == null )
            {
                throw base.GetConvertFromException( value );
            }

            string text = value as string;
            if( text == null )
            {
                return base.ConvertFrom( context, culture, value );
            }

            int year;
            if( int.TryParse( text, out year ) )
            {
                return new YearPeriod( year );
            }

            var converter = new DateTimeConverter();
            return new DayPeriod( ( DateTime )converter.ConvertFrom( text ) );
        }

        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
        {
            throw new NotImplementedException();
        }

        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType )
        {
            throw new NotImplementedException();
        }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value == null )
            {
                return null;
            }

            var yearPeriod = value as YearPeriod;
            if( yearPeriod != null )
            {
                return yearPeriod.Year;
            }

            var dayPeriod = value as DayPeriod;
            if( dayPeriod != null )
            {
                return string.Format( "{0}-{1}-{2}",
                    dayPeriod.Day.Year,
                    ( dayPeriod.Day.Month < 10 ? "0" : string.Empty ) + dayPeriod.Day.Month,
                   ( dayPeriod.Day.Day < 10 ? "0" : string.Empty ) + dayPeriod.Day.Day );
            }

            throw new NotSupportedException( "Cannot convert type: " + value.GetType() );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return ConvertFrom( value );
        }
    }
}
