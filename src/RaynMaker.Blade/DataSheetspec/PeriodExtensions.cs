using System;
using Plainion;

namespace RaynMaker.Blade.DataSheetSpec
{
    public static class PeriodExtensions
    {
        public static int Year( this IPeriod self )
        {
            Contract.RequiresNotNull( self, "self" );

            var yearPeriod = self as YearPeriod;
            if( yearPeriod != null )
            {
                return yearPeriod.Year;
            }

            var dayPeriod = self as DayPeriod;
            if( dayPeriod != null )
            {
                return dayPeriod.Date.Year;
            }

            throw new NotSupportedException( "Unsupported period type: " + self.GetType().Name );
        }
    }
}
