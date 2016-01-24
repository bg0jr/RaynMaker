using System.Collections.Generic;
using System.Linq;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Analysis.AnalysisSpec
{
    class Functions
    {
        public static IFigure Average( IEnumerable<IFigure> series )
        {
            if( !series.Any() )
            {
                return null;
            }

            var result = new DerivedFigure()
            {
                Value = series.Average( d => d.Value )
            };

            var currencyDatums = series.OfType<ICurrencyFigure>();
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
        public static IFigure Growth( IEnumerable<IFigure> series )
        {
            if( !series.Any() )
            {
                return null;
            }

            var sortedSeries = series
                .OrderBy( d => d.Period )
                .ToList();

            var rates = new List<double>( sortedSeries.Count - 1 );
            for( int i = 1; i < sortedSeries.Count; ++i )
            {
                rates.Add( Growth( sortedSeries[ i - 1 ].Value.Value, sortedSeries[ i ].Value.Value ) );
            }

            var result = new DerivedFigure()
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

        public static IEnumerable<IFigure> LastN( IEnumerable<IFigure> series, int count )
        {
            if( series.Count() < count )
            {
                return series;
            }

            return series
                .OrderBy( v => v.Period )
                .Skip( series.Count() - count );
        }

        public static IFigure Last( IEnumerable<IFigure> series )
        {
            return series
                .OrderBy( v => v.Period )
                .LastOrDefault();
        }
    }
}
