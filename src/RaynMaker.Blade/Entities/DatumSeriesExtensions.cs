using System.Linq;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Entities
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
                .First();
        }

        public static TDatumType Current<TDatumType>( this IDatumSeries self )
        {
            return ( TDatumType )self.Current();
        }
    }
}
