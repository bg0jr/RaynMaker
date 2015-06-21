using System.Collections.Generic;
using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    class DatumProvider
    {
        private Asset myAsset;

        public DatumProvider( Asset asset )
        {
            myAsset = asset;
        }

        public IEnumerable<T> GetSeries<T>()
        {
            var series = myAsset.Data.OfType<Series>()
                .Where( s => s.Values.OfType<T>().Any() )
                .SingleOrDefault();

            if( series == null )
            {
                return Enumerable.Empty<T>();
            }

            return series.Values.Cast<T>();
        }
    }
}
