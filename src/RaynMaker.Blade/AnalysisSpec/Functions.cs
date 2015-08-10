using System.Collections.Generic;
using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.AnalysisSpec
{
    class Functions
    {
        public static IDatum Average( IEnumerable<IDatum> series )
        {
            var result = new DerivedDatum()
            {
                Value = series.Average( d => d.Value )
            };

            var currencyDatums = series.OfType<ICurrencyDatum>();
            if ( currencyDatums.Any( ))
            {
                result.Currency = currencyDatums.First().Currency;
            }
            
            result.Inputs.AddRange( series );
            
            return result;
        }

        public static IEnumerable<IDatum> Last( IEnumerable<IDatum> series, int count )
        {
            if( series.Count() < count )
            {
                return series;
            }

            return series.Skip( series.Count() - count );
        }
    }
}
