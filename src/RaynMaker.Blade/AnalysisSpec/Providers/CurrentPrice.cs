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
            return context.Asset.SeriesOf( typeof( Price ) ).Current();
        }
    }
}
