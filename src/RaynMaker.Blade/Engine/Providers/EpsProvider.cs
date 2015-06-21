using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public class EpsProvider : IFigureProvider
    {
        public string Name { get { return "Eps"; } }

        public object ProvideValue( Asset asset )
        {
            return asset.Data.OfType<Series>()
                .Where( s => s.Values.OfType<Eps>().Any() )
                .SingleOrDefault();
        }
    }
}
