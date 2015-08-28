using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blade
{
    /// <summary>
    /// Simple Design-By-Contract helper.
    /// <remarks>
    /// Uses lambda expressions to implement asserts and pre-conditions.
    /// If an assert or a pre-conditions fails an exception is thrown. This exception
    /// either contains a given message or the failing expression itselv as
    /// string representation.
    /// </remarks>
    /// <example>
    /// <code>
    /// public void DoIt( string s )
    /// {
    ///     this.Require( x =&gt; !string.IsNullOrEmpty( s ) );
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public static class DbC
    {
        /// <summary>
        /// Checks the given condition.
        /// </summary>
        [Conditional( "DBC_CHECK_ALL" )]
        public static void Assert<T>( this T obj, Expression<Func<T, bool>> pred )
        {
            obj.Require( pred, null );
        }
        /// <summary>
        /// Checks the given condition and throws the given message if it fails.
        /// </summary>
        [Conditional( "DBC_CHECK_ALL" )]
        public static void Assert<T>( this T obj, Expression<Func<T, bool>> pred, string msg )
        {
            obj.Require( pred, msg );
        }
        /// <summary>
        /// Checks the given condition.
        /// </summary>
        public static void Require<T>( this T obj, Expression<Func<T, bool>> pred )
        {
            obj.Require( pred, null );
        }
        /// <summary>
        /// Checks the given condition and throws the given message if it fails.
        /// </summary>
        public static void Require<T>( this T obj, Expression<Func<T, bool>> pred, string msg )
        {
            Func<T, bool> func = pred.Compile();
            if( func( obj ) )
            {
                return;
            }
            if( msg == null )
            {
                throw new ArgumentException( "Pre-condition failed: " + pred.ToString() );
            }
            throw new ArgumentException( "Pre-condition failed: " + msg );
        }
    }
}
