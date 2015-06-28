using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public class SharesOutstandingProvider : IFigureProvider
    {
        public string Name { get { return "SharesOutstanding"; } }

        public object ProvideValue( Asset asset )
        {
            return asset.Data.OfType<Series>()
                .Where( s => s.Values.OfType<SharesOutstanding>().Any() )
                .SingleOrDefault() ?? new Series();
        }
    }
}
