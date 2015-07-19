using System.Collections.Generic;
using System.Linq;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;

namespace RaynMaker.Blade.Engine
{
    public class MarketCapProvider : IFigureProvider
    {
        public string Name { get { return "MarketCap"; } }

        public object ProvideValue( Asset asset )
        {
            var price = asset.Data.OfType<Price>().SingleOrDefault();
            if( price == null )
            {
                return null;
            }

            var provider = new DatumProvider( asset );
            var allShares = provider.GetSeries<SharesOutstanding>();

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
