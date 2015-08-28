using System;
using System.Data;
using System.Linq;
using Blade.Data;
using Blade.Collections;

namespace RaynMaker.Import
{
    /// <summary>
    /// Implements a policy which checks the result data against
    /// a given date range.
    /// If "From" or "To" is not set/returned the result is treated
    /// as valid if there is at least one row.
    /// </summary>
    public class ValidRangePolicy : FirstNonNullPolicy
    {
        public ValidRangePolicy( DateTime? from, DateTime? to )
        {
            From = () => from;
            To = () => to;
        }

        public ValidRangePolicy( Func<DateTime?> from, Func<DateTime?> to )
        {
            From = from;
            To = to;
        }

        public Func<DateTime?> From { get; private set; }
        public Func<DateTime?> To { get; private set; }

        protected override bool Validate( DataTable result )
        {
            if ( !base.Validate( result ) )
            {
                return false;
            }

            var from = From();
            var to = To();

            if ( from == null || to == null )
            {
                return result.Rows.Count > 0;
            }

            var columns = result.Columns.ToSet();
            var dateCol = columns.FirstOrDefault( c => c.IsDateColumn() );
            if ( dateCol == null )
            {
                throw new Exception( "Could not get date column from table: " + columns.ToHuman() );
            }

            // HACK: now we only check first and last
            // lets match exact here - otherwise we would never get the current price
            var isResultValid = false;
            var dates = result.Rows.ToSet().Select( r => r[ dateCol.ColumnName ] ).OrderBy( d => d ).ToList();
            var first = dates.First();
            var last = dates.Last();

            if ( dateCol.DataType == typeof( DateTime ) )
            {
                isResultValid = from.Value >= (DateTime)first && to.Value <= (DateTime)last;
            }
            else
            {
                isResultValid = from.Value.Year >= (int)first && to.Value.Year <= (int)last;
            }

            return isResultValid;
        }
    }
}
