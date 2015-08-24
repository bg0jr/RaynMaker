using System.Collections.Generic;

namespace RaynMaker.Entities
{
    public class DatumByPeriodComparer : IComparer<IDatum>
    {
        public int Compare( IDatum x, IDatum y )
        {
            return x.Period.CompareTo( y.Period );
        }
    }
}
