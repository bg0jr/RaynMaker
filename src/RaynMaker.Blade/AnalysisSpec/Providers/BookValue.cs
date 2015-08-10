using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    /// <summary>
    /// BookValue = Equity / SharesOutstanding
    /// </summary>
    public class BookValue : IFigureProvider
    {
        public string Name { get { return ProviderNames.BookValue; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var allEquity = context.GetDatumSeries<Equity>();
            var allShares = context.GetDatumSeries<SharesOutstanding>();

            if( !allEquity.Any() || !allShares.Any() )
            {
                return new Series();
            }

            var result = new Series();

            foreach( var equity in allEquity )
            {
                var shares = allShares.SingleOrDefault( e => e.Year == equity.Year );
                if( shares != null )
                {
                    var eps = new DerivedDatum
                    {
                        Year = equity.Year,
                        Currency = equity.Currency,
                        Value = equity.Value / shares.Value
                    };
                    eps.Inputs.Add( equity );
                    eps.Inputs.Add( shares );
                    result.Values.Add( eps );
                }
            }

            return result;
        }
    }
}
