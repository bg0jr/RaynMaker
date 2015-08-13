using System;
using System.ComponentModel;
using System.Globalization;

namespace RaynMaker.Blade.Entities
{
    public sealed class PeriodConverter : TypeConverter
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
    }
}
