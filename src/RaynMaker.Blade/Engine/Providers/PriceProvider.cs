using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;

namespace RaynMaker.Blade.Engine.Providers
{
    public class PriceProvider : IFigureProvider
    {
        public string Name { get { return "Price"; } }

        public object ProvideValue( Asset asset )
        {
            return asset.Data.OfType<Price>().SingleOrDefault();
        }
    }
}
