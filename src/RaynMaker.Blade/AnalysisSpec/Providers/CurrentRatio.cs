using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public class CurrentRatio : IFigureProvider
    {
        public string Name { get { return ProviderNames.CurrentRatio; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var allAssets = context.GetDatumSeries<Assets>();
            var allLiabilities = context.GetDatumSeries<Liabilities>();

            if( !allAssets.Any() || !allLiabilities.Any() )
            {
                return null;
            }

            var assets = allAssets
                .OrderByDescending( a => a.Year )
                .First();

            var liabilities = allLiabilities.SingleOrDefault( d => d.Year == assets.Year );
            if( liabilities == null )
            {
                return null;
            }

            var result = new DerivedDatum
            {
                Year = assets.Year,
                Value = assets.Value / liabilities.Value
            };
            result.Inputs.Add( assets );
            result.Inputs.Add( liabilities );
            return result;
        }
    }
}
