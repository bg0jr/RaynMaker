using System;
using System.ComponentModel;
using System.Globalization;

namespace RaynMaker.Blade.Entities
{
    public sealed class CurrencyConverter : TypeConverter
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
            if( text != null )
            {
                return Currencies.Parse( text );
            }

            return base.ConvertFrom( context, culture, value );
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
