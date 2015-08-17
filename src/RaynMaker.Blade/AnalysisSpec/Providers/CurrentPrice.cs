using System.Linq;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public class CurrentPrice : IFigureProvider
    {
        public string Name { get { return ProviderNames.CurrentPrice; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var series = context.Asset.Data.OfType<IDatumSeries>()
                .Where( s => s.DatumType == typeof( Price ) )
                .SingleOrDefault();

            if( series == null || !series.Any() )
            {
                return null;
            }

            return series
                .OrderByDescending( v => v.Period )
                .First();
        }
    }
}
