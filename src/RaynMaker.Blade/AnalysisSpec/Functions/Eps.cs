using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Functions
{
    /// <summary>
    /// Earnings per share
    /// </summary>
    public class Eps : IFigureProvider
    {
        public string Name { get { return FunctionNames.Eps; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var allNetIncome = context.GetDatumSeries<NetIncome>();
            var allShares = context.GetDatumSeries<SharesOutstanding>();

            if( !allNetIncome.Any() || !allShares.Any() )
            {
                return new Series();
            }

            var result = new Series();

            foreach( var netIncome in allNetIncome )
            {
                var shares = allShares.SingleOrDefault( e => e.Year == netIncome.Year );
                if( shares != null )
                {
                    var eps = new DerivedDatum
                    {
                        Year = netIncome.Year,
                        Currency = netIncome.Currency,
                        Value = netIncome.Value / shares.Value
                    };
                    eps.Inputs.Add( netIncome );
                    eps.Inputs.Add( shares );
                    result.Values.Add( eps );
                }
            }

            return result;
        }
    }
}
