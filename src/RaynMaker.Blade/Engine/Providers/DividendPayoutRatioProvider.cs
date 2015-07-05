using System.Collections.Generic;
using System.Linq;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public class DividendPayoutRatioProvider : IFigureProvider
    {
        public string Name { get { return "DividendPayoutRatio"; } }

        public object ProvideValue( Asset asset )
        {
            var provider = new DatumProvider( asset );

            var dividends = provider.GetSeries<Dividend>();
            var earnings = provider.GetSeries<Eps>();

            if( !dividends.Any() || !earnings.Any() )
            {
                return new Series();
            }

            provider.EnsureCurrencyConsistency( dividends, earnings );

            var result = new Series();

            foreach( var dividend in dividends )
            {
                var eps = earnings.SingleOrDefault( e => e.Year == dividend.Year );
                if( eps != null )
                {
                    result.Values.Add( new DerivedDatum
                    {
                        Year = dividend.Year,
                        Currency = dividend.Currency,
                        Value = dividend.Value / eps.Value * 100
                    } );
                }
            }

            return result;
        }
    }
}
