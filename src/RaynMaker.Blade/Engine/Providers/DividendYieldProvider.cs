using System.Linq;
using Plainion;
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

            var provider = new DatumProvider( asset );
            var dividends = provider.GetSeries<Dividend>();

            var dividend = dividends.SingleOrDefault( d => d.Year == price.Date.Year );
            if( dividend == null )
            {
                // TODO: info got lost that this is based on dividend of previous year
                dividend = dividends.SingleOrDefault( d => d.Year == price.Date.Year - 1 );
                if( dividend == null )
                {
                    return null;
                }
            }

            Contract.Requires( price.Currency == dividend.Currency, "Currency mismatch" );

            return new DerivedDatum
            {
                Date = price.Date,
                Currency = dividend.Currency,
                Value = dividend.Value / price.Value * 100
            };
        }
    }
}
