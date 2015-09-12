using System;
using System.Collections.Generic;
using System.Data;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Extensions to System.Data.DataColumn.
    /// </summary>
    public static class DataColumnExtensions
    {
        /// <summary>
        /// Returns an IEnumerableT of the given DataColumnCollection.
        /// </summary>
        public static IEnumerable<DataColumn> ToSet( this DataColumnCollection list )
        {
            foreach( DataColumn dataColumn in list )
            {
                yield return dataColumn;
            }
            yield break;
        }
        /// <summary>
        /// Returns the sorted set of all indices of the given list.
        /// </summary>
        public static IEnumerable<int> Indices( this DataColumnCollection list )
        {
            for( int i = 0; i < list.Count; i++ )
            {
                yield return i;
            }
            yield break;
        }

        /// <summary/>
        public static bool IsDateColumn( this DataColumn column )
        {
            return ( column.ColumnName.Equals( "year", StringComparison.OrdinalIgnoreCase ) ||
                     column.ColumnName.Equals( "date", StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
