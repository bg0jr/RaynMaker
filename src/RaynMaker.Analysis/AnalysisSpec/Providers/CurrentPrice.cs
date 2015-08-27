using RaynMaker.Blade.Engine;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public class CurrentPrice : IFigureProvider
    {
        public string Name { get { return ProviderNames.CurrentPrice; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            return context.Data.SeriesOf( typeof( Price ) ).Current();
        }
    }
}
