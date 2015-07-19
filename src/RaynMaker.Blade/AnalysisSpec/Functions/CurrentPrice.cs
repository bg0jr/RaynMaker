using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Functions
{
    public class CurrentPrice : IFigureProvider
    {
        public string Name { get { return FunctionNames.CurrentPrice; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            return context.Asset.Data.OfType<Price>().SingleOrDefault();
        }
    }
}
