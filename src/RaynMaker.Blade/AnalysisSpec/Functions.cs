using System.Collections.Generic;
using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.AnalysisSpec
{
    class Functions
    {
        public static double Average( IEnumerable<IDatum> series )
        {
            return series.Average( d => d.Value );
        }

        public static IEnumerable<IDatum> Last( IEnumerable<IDatum> series, int count )
        {
            if ( series.Count() < count)
            {
                return series;
            }

            return series.Skip( series.Count() - count );
        }
    }
}
