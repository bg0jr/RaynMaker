using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public class DividendPayoutRatioProvider : IFigureProvider
    {
        public string Name { get { return "DividendPayoutRatio"; } }

        public object ProvideValue( Asset asset )
        {
            var dividendSeries = asset.Data.OfType<Series>()
                .Where( s => s.Values.OfType<Dividend>().Any() )
                .SingleOrDefault();

            var epsSeries = asset.Data.OfType<Series>()
                .Where( s => s.Values.OfType<Eps>().Any() )
                .SingleOrDefault();

            if( dividendSeries == null || epsSeries == null )
            {
                return new Series();
            }

            var result = new Series();
            var epsValues = epsSeries.Values.Cast<Eps>();

            foreach( var dividend in dividendSeries.Values.Cast<Dividend>() )
            {
                var eps = epsValues.SingleOrDefault( e => e.Year == dividend.Year );
                if( eps != null )
                {
                    result.Values.Add( new AnualDatum
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
