using System;
using System.Collections.Generic;
using System.Linq;
using RaynMaker.Entities;

namespace RaynMaker.Entities
{
    public static class DatumSeriesExtensions
    {
        public static IDatum Current( this IDatumSeries self )
        {
            if( self == null )
            {
                return null;
            }

            return self
                .OrderByDescending( v => v.Period )
                .FirstOrDefault();
        }

        public static TDatumType Current<TDatumType>( this IDatumSeries self )
        {
            return ( TDatumType )self.Current();
        }

        public static IDatumSeries SeriesOf( this IEnumerable<IDatumSeries> self, Type datumType )
        {
            return self.OfType<IDatumSeries>()
                .Where( s => s.DatumType == datumType )
                .SingleOrDefault();
        }
    }
}
