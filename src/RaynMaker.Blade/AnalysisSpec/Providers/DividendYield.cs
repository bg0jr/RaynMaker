using System.Linq;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public class DividendYield : IFigureProvider
    {
        public string Name { get { return ProviderNames.DividendYield; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var price = context.Asset.Data.OfType<Price>().SingleOrDefault();
            if( price == null )
            {
                return null;
            }

            var dividends = context.GetDatumSeries<Dividend>();
            var allShares = context.GetDatumSeries<SharesOutstanding>();

            var dividend = dividends.SingleOrDefault( d => d.Year == price.Date.Year );
            var shares = allShares.SingleOrDefault( s => s.Year == price.Date.Year );
            if( dividend == null )
            {
                dividend = dividends.SingleOrDefault( d => d.Year == price.Date.Year - 1 );
                if( dividend == null )
                {
                    return null;
                }
                else
                {
                    shares = allShares.SingleOrDefault( s => s.Year == price.Date.Year - 1 );
                    Contract.Requires( shares != null, "Failed to fetch SharesOutstanding for {0}", price.Date.Year - 1 );
                }
            }
            else
            {
                Contract.Requires( shares != null, "Failed to fetch SharesOutstanding for {0}", price.Date.Year );
            }

            Contract.Requires( price.Currency == dividend.Currency, "Currency mismatch" );

            var result = new DerivedDatum
            {
                Date = price.Date,
                Value = dividend.Value / shares.Value / price.Value * 100
            };
            result.Inputs.Add( dividend );
            result.Inputs.Add( price );
            return result;
        }
    }
}
