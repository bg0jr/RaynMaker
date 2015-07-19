using System.Collections.Generic;
using System.Linq;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public class DividendPayoutRatio : IFigureProvider
    {
        public string Name { get { return ProviderNames.DividendPayoutRatio; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var dividends = context.GetDatumSeries<Dividend>();
            var earnings = context.GetCalculatedSeries<IAnualFinancialDatum>( ProviderNames.Eps );

            if( !dividends.Any() || !earnings.Any() )
            {
                return new Series();
            }

            context.EnsureCurrencyConsistency( dividends, earnings );

            var result = new Series();

            foreach( var dividend in dividends )
            {
                var eps = earnings.SingleOrDefault( e => e.Year == dividend.Year );
                if( eps != null )
                {
                    var ratio = new DerivedDatum
                    {
                        Year = dividend.Year,
                        Value = dividend.Value / eps.Value * 100
                    };
                    ratio.Inputs.Add( dividend );
                    ratio.Inputs.Add( eps );
                    result.Values.Add( ratio );
                }
            }

            return result;
        }
    }
}
