using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public class MarketCap : IFigureProvider
    {
        public string Name { get { return ProviderNames.MarketCap; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var price = context.Asset.Data.OfType<Price>().SingleOrDefault();
            if( price == null )
            {
                return null;
            }

            var allShares = context.GetDatumSeries<SharesOutstanding>();

            var shares = allShares.SingleOrDefault( d => d.Year == price.Date.Year );
            if( shares == null )
            {
                shares = allShares.SingleOrDefault( d => d.Year == price.Date.Year - 1 );
                if( shares == null )
                {
                    return null;
                }
            }

            var result = new DerivedDatum
            {
                Date = price.Date,
                Currency = price.Currency,
                Value = shares.Value * price.Value
            };
            result.Inputs.Add( shares );
            result.Inputs.Add( price );
            return result;
        }
    }
}
