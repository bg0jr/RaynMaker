using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Functions
{
    public class CurrentPrice : IFigureProvider
    {
        public string Name { get { return "Price"; } }

        public object ProvideValue( Asset asset )
        {
            return asset.Data.OfType<Price>().SingleOrDefault();
        }
    }
}
