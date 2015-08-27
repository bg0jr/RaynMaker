using System;
using System.Windows;
using System.Windows.Markup;

namespace RaynMaker.Blade.AnalysisSpec
{
    [MarkupExtensionReturnType( typeof( double ) )]
    public class InMioExtension : MarkupExtension
    {
        public InMioExtension( double value )
        {
            Value = value;
        }

        [ConstructorArgument( "value" )]
        public double Value { get; set; }

        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            return Value * 1000000;
        }
    }
}
