using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public class DividendProvider : IFigureProvider
    {
        public string Name { get { return "Dividend"; } }

        public object ProvideValue( Asset asset )
        {
            return asset.Data.OfType<Series>()
                .Where( s => s.Values.OfType<Dividend>().Any() )
                .SingleOrDefault() ?? new Series();
        }
    }
}
