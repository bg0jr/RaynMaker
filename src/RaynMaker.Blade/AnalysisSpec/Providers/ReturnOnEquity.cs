using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public class ReturnOnEquity : IFigureProvider
    {
        public string Name { get { return ProviderNames.ReturnOnEquity; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var allEarnings = context.GetCalculatedSeries<IAnualFinancialDatum>(ProviderNames.Eps);
            var allBookValues = context.GetCalculatedSeries<IAnualFinancialDatum>( ProviderNames.BookValue );

            if( !allEarnings.Any() || !allBookValues.Any() )
            {
                return new Series();
            }

            var result = new Series();

            foreach( var earnings in allEarnings )
            {
                var bookValue = allBookValues.SingleOrDefault( e => e.Year == earnings.Year );
                if( bookValue != null )
                {
                    var eps = new DerivedDatum
                    {
                        Year = earnings.Year,
                        Value = earnings.Value / bookValue.Value
                    };
                    eps.Inputs.Add( earnings );
                    eps.Inputs.Add( bookValue );
                    result.Values.Add( eps );
                }
            }

            return result;
        }
    }
}
