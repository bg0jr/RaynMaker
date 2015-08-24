using System.Collections.Generic;
using System.Linq;
using RaynMaker.Blade.Entities;
using RaynMaker.Entities;

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
            if( currencyDatums.Any() )
            {
                result.Currency = currencyDatums.First().Currency;
            }

            result.Inputs.AddRange( series );

            return result;
        }

        /// <summary>
        /// Returns growth rate in percentage.
        /// </summary>
        public static IDatum Growth( IEnumerable<IDatum> series )
        {
            var sortedSeries = series
                .OrderBy( d => d.Period )
                .ToList();

            var rates = new List<double>( sortedSeries.Count - 1 );
            for( int i = 1; i < sortedSeries.Count; ++i )
            {
                rates.Add( Growth( sortedSeries[ i - 1 ].Value.Value, sortedSeries[ i ].Value.Value ) );
            }

            var result = new DerivedDatum()
            {
                Value = rates.Average()
            };

            result.Inputs.AddRange( series );

            return result;
        }

        private static double Growth( double oldValue, double newValue )
        {
            return ( newValue - oldValue ) / oldValue * 100;
        }

        public static IEnumerable<IDatum> LastN( IEnumerable<IDatum> series, int count )
        {
            if( series.Count() < count )
            {
                return series;
            }

            return series
                .OrderBy( v => v.Period )
                .Skip( series.Count() - count );
        }

        public static IDatum Last( IEnumerable<IDatum> series )
        {
            return series
                .OrderBy( v => v.Period )
                .Last();
        }
    }
}
