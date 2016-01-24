using System;
using System.Collections.Generic;
using System.Linq;
using RaynMaker.Entities;

namespace RaynMaker.Entities
{
    public static class FigureSeriesExtensions
    {
        public static IFigure Current( this IFigureSeries self )
        {
            if( self == null )
            {
                return null;
            }

            return self
                .OrderByDescending( v => v.Period )
                .FirstOrDefault();
        }

        public static TFigureType Current<TFigureType>( this IFigureSeries self )
        {
            return ( TFigureType )self.Current();
        }

        public static IFigureSeries SeriesOf( this IEnumerable<IFigureSeries> self, Type figureType )
        {
            return self.OfType<IFigureSeries>()
                .Where( s => s.FigureType == figureType )
                .SingleOrDefault();
        }
    }
}
