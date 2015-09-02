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
        /// Removes the elements of the given list for which the predicate returns true.
        /// </summary>
        public static void RemoveIf<T>( this IList<T> list, Func<T, bool> pred )
        {
            list.Where( pred ).ToList<T>().Foreach( delegate( T x )
            {
                list.Remove( x );
            } );
        }
        /// <summary>
        /// Returns the first element of the given list for which pred returns true.
        /// If nothing was found the given defaultValue will be returned.
        /// </summary>
        public static T FirstOrDefault<T>( this IEnumerable<T> list, Predicate<T> pred, T defaultValue )
        {
            foreach( T current in list )
            {
                if( pred( current ) )
                {
                    return current;
                }
            }
            return defaultValue;
        }
        /// <summary>
        /// Returns concatenated enumerable of the given values.
        /// </summary>
        public static IEnumerable<T> Concat<T>( this IEnumerable<T> list, params T[] values )
        {
            foreach( T current in list )
            {
                yield return current;
            }
            try
            {
                for( int i = 0; i < values.Length; i++ )
                {
                    T t = values[ i ];
                    yield return t;
                }
            }
            finally
            {
            }
            yield break;
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
        /// <summary>
        /// Substracts the right-hand-side array from the left-hand-site IEnumerable.
        /// </summary>
        public static IEnumerable<T> Substract<T>( this IEnumerable<T> lhs_in, params T[] rhs_in )
        {
            return
                from e in lhs_in
                where !rhs_in.Contains( e )
                select e;
        }
        /// <summary>
        /// Substracts the right-hand-side IEnumerable from the left-hand-site IEnumerable.
        /// </summary>
        public static IEnumerable<T> Substract<T>( this IEnumerable<T> lhs_in, IEnumerable<T> rhs_in )
        {
            return
                from e in lhs_in
                where !rhs_in.Contains( e )
                select e;
        }
        /// <summary>
        /// Checks whether the given two arrays are equal. Means: both
        /// arrays contain the same elements independent of the element 
        /// order in the arrays.
        /// </summary>
        public static bool IsSame<T>( this IEnumerable<T> lhs_in, IEnumerable<T> rhs_in )
        {
            return lhs_in.All( ( T e ) => rhs_in.Contains( e ) ) && rhs_in.All( ( T e ) => lhs_in.Contains( e ) );
        }
        /// <summary>
        /// Returns a IList representation of the given list or null if the 
        /// argument is null.
        /// </summary>
        public static IList<T> ToListOrDefault<T>( this IEnumerable<T> list )
        {
            if( list == null )
            {
                return null;
            }
            return list.ToList<T>();
        }
        /// <summary>
        /// Returns a Queue representation of the given list or null if the 
        /// argument is null.
        /// </summary>
        public static Queue<T> ToQueueOrDefault<T>( this IEnumerable<T> list )
        {
            if( list == null )
            {
                return null;
            }
            return new Queue<T>( list );
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
        /// Intersects all sets of the given super set.
        /// </summary>
        /// <returns>null if super set is null, empty set if super set is empty</returns>
        public static IEnumerable<T> IntersectAll<T>( this IEnumerable<IEnumerable<T>> superSet )
        {
            if( superSet == null )
            {
                return null;
            }
            if( !superSet.Any<IEnumerable<T>>() )
            {
                return new List<T>();
            }
            IEnumerable<T> enumerable = superSet.First<IEnumerable<T>>();
            foreach( IEnumerable<T> current in superSet.Skip( 1 ) )
            {
                enumerable = enumerable.Intersect( current );
            }
            return enumerable;
        }
        /// <summary>
        /// Returns elements from the sequence until the condition is true.
        /// The element for which the condition became true will also be returned.
        /// </summary>
        public static IEnumerable<T> TakeUntil<T>( this IEnumerable<T> list, Func<T, bool> predicate )
        {
            foreach( T current in list )
            {
                yield return current;
                if( predicate( current ) )
                {
                    yield break;
                }
            }
            yield break;
        }
        /// <summary />
        public static bool IsEmpty<T>( this IEnumerable<T> list )
        {
            return !list.Any<T>();
        }

        /// <summary>
        /// Filers the set by the given type.
        /// </summary>
        public static IEnumerable Is<T>( this IEnumerable list )
        {
            foreach( object current in list )
            {
                if( current is T )
                {
                    yield return current;
                }
            }
            yield break;
        }
        /// <summary>
        /// Loops over the given set and executes the given action for each
        /// item in the set. Passes the item itself and its index to the action.
        /// </summary>
        public static void Foreach( this IEnumerable list, Action<object> action )
        {
            foreach( object current in list )
            {
                action( current );
            }
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
