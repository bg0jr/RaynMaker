using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using RaynMaker.Import;
using System.Globalization;

namespace RaynMaker.Import.Web
{
    class GenericTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
        {
            if ( destinationType == typeof( string ) )
            {
                return true;
            }

            return base.CanConvertTo( context, destinationType );
        }

        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType )
        {
            if( value == null )
            {
                return null;
            }

            if ( destinationType == typeof( string ) )
            {
                return value.GetType().Name;
            }

            return base.ConvertTo( context, culture, value, destinationType );
        }
        public override PropertyDescriptorCollection GetProperties( ITypeDescriptorContext context, object value, Attribute[] attributes )
        {
            return TypeDescriptor.GetProperties(value.GetType(), attributes );
        }

        public override bool GetPropertiesSupported( ITypeDescriptorContext context )
        {
            return true;
        }
    }
}
