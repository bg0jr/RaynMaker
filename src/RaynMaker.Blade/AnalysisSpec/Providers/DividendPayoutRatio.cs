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
            var allNetIncome = context.GetDatumSeries<NetIncome>();

            if( !dividends.Any() || !allNetIncome.Any() )
            {
                return new Series();
            }

            context.EnsureCurrencyConsistency( dividends, allNetIncome );

            var result = new Series();

            foreach( var dividend in dividends )
            {
                var netIncome = allNetIncome.SingleOrDefault( e => e.Year == dividend.Year );
                if( netIncome != null )
                {
                    var ratio = new DerivedDatum
                    {
                        Year = dividend.Year,
                        Value = dividend.Value / netIncome.Value * 100
                    };
                    ratio.Inputs.Add( dividend );
                    ratio.Inputs.Add( netIncome );
                    result.Values.Add( ratio );
                }
            }

            return result;
        }
    }
}
