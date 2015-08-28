using System.Collections.Generic;
using System.Data;
using System.Linq;
using Blade.Data;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    /// <summary>
    /// Implements a policy which accepts the first non-null result.
    /// </summary>
    public class FirstNonNullPolicy : IResultPolicy
    {
        public DataTable ResultTable { get; protected set; }
        public IEnumerable<Site> Sites { get; protected set; }

        public virtual bool Validate( Site site, DataTable result )
        {
            bool isValid = Validate( result );
            if ( isValid )
            {
                Sites = new Site[] { site };
                ResultTable = result;
            }

            return isValid;
        }

        protected virtual bool Validate( DataTable result )
        {
            if ( result == null )
            {
                return false;
            }

            // if table has now rows and columns it is also "no result"
            if ( result.Rows.Count == 0 || result.Columns.Count == 0 )
            {
                return false;
            }

            // if table is empty (at least first line -> "no result"
            if ( result.Rows.Count == 1 &&
                result.Columns.ToSet().All( col => result.IsEmptyCell( result.Rows[ 0 ][ col ] ) ) )
            {
                return false;
            }

            return true;
        }
    }
}
