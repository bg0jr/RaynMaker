using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RaynMaker.Blade.Sdk
{
    //public sealed class CurrencyConverter : TypeConverter
    //{
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    //    }

    //    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    //    {
    //        if (!(destinationType == typeof(string)))
    //        {
    //            return base.CanConvertTo(context, destinationType);
    //        }
    //        if (context == null || context.Instance == null)
    //        {
    //            return true;
    //        }
    //        if (!(context.Instance is Brush))
    //        {
    //            throw new ArgumentException(SR.Get("General_Expected_Type", new object[]
    //            {
    //                "Brush"
    //            }), "context");
    //        }
    //        Brush brush = (Brush)context.Instance;
    //        return brush.CanSerializeToString();
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        if (value == null)
    //        {
    //            throw base.GetConvertFromException(value);
    //        }
    //        string text = value as string;
    //        if (text != null)
    //        {
    //            return Brush.Parse(text, context);
    //        }
    //        return base.ConvertFrom(context, culture, value);
    //    }

    //    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    //    {
    //        if (destinationType != null && value is Brush)
    //        {
    //            Brush brush = (Brush)value;
    //            if (destinationType == typeof(string))
    //            {
    //                if (context != null && context.Instance != null && !brush.CanSerializeToString())
    //                {
    //                    throw new NotSupportedException(SR.Get("Converter_ConvertToNotSupported"));
    //                }
    //                return brush.ConvertToString(null, culture);
    //            }
    //        }
    //        return base.ConvertTo(context, culture, value, destinationType);
    //    }
    //}
}
