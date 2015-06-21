using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
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
