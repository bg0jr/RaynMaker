using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Collections
{
    /// <summary>
    /// Additional extensions to generic IEnumerable interface.
    /// </summary>
    public static class IEnumerableTExtensions
    {
        /// <summary>
        /// Joins the given list to a string.
        /// </summary>
        public static string Join<T, TSep>( this IEnumerable<T> list, TSep sep )
        {
            if( list == null )
            {
                return null;
            }
            T t = list.FirstOrDefault<T>();
            if( object.Equals( t, default( T ) ) )
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append( t.ToString() );
            list.Skip( 1 ).Foreach( delegate( T item )
            {
                sb.Append( sep );
                sb.Append( item.ToString() );
            } );
            return sb.ToString();
        }
        /// <summary>
        /// Loops over the given set and executes the given action for each
        /// item in the set.
        /// </summary>
        public static void Foreach<T>( this IEnumerable<T> list, Action<T> action )
        {
            foreach( T current in list )
            {
                action( current );
            }
        }
        /// <summary>
        /// Loops over the given set and executes the given action for each
        /// item in the set. Passes the item itself and its index to the action.
        /// </summary>
        public static void ForeachIndex<T>( this IEnumerable<T> list, Action<T, int> action )
        {
            int num = 0;
            foreach( T current in list )
            {
                action( current, num++ );
            }
        }

        /// <summary>
        /// Concats the given input lists.
        /// The input lists are not modified.
        /// </summary>
        /// <returns><c>lhs_in</c> + <c>rhs_in</c></returns>
        public static IEnumerable<T> Concat<T>( this IEnumerable<T> list, params IEnumerable<T>[] lists_in )
        {
            foreach( T current in list )
            {
                yield return current;
            }
            try
            {
                for( int i = 0; i < lists_in.Length; i++ )
                {
                    IEnumerable<T> enumerable = lists_in[ i ];
                    foreach( T current2 in enumerable )
                    {
                        yield return current2;
                    }
                }
            }
            finally
            {
            }
            yield break;
        }

        /// <summary />
        public static int IndexOf<T>( this IEnumerable<T> list, Func<T, bool> pred )
        {
            int num = 0;
            foreach( T current in list )
            {
                if( pred( current ) )
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        /// <summary>
        /// Converts the given set into a generic one so that
        /// it can be used with LinQ.
        /// </summary>
        public static IEnumerable<object> ToGeneric( this IEnumerable list )
        {
            return list.OfType<object>();
        }
        /// <summary>
        /// Dumps the given list into a string in human readable format.
        /// </summary>
        public static string ToHuman( this IEnumerable list )
        {
            if( list == null )
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append( "[" );
            stringBuilder.Append( string.Join( "|", (
                from a in list.ToGeneric()
                select a.ToString() ).ToArray<string>() ) );
            stringBuilder.Append( "]" );
            return stringBuilder.ToString();
        }
    }
}
