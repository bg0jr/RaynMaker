using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine.Providers
{
    public class DividendYieldProvider : IFigureProvider
    {
        public string Name { get { return "DividendYield"; } }

        public object ProvideValue( Asset asset )
        {
            var price = asset.Data.OfType<Price>().SingleOrDefault();
            if( price == null )
            {
                return null;
            }

            var dividendSeries = asset.Data.OfType<Series>()
                .Where( s => s.Values.OfType<Dividend>().Any() )
                .SingleOrDefault();

            if( dividendSeries == null )
            {
                return null;
            }

            var dividend = dividendSeries.Values.Cast<Dividend>().SingleOrDefault( d => d.Year == price.Date.Year );
            if( dividend == null )
            {
                // TODO: info got lost that this is based on dividend of previous year
                dividend = dividendSeries.Values.Cast<Dividend>().SingleOrDefault( d => d.Year == price.Date.Year - 1 );
                if( dividend == null )
                {
                    return null;
                }
            }

            return new DailyDatum
            {
                Date = price.Date,
                Value = dividend.Value / price.Value * 100
            };
        }
    }
}
