using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade
{
    /// <summary>
    /// Extensions to System.Text.StringBuilder.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Adds the given item to this StringBuilder.
        /// <remarks>
        /// This method wraps StringBuilder.Append and does not return any
        /// return value so that it can be used as delegate easier in some
        /// cases.
        /// </remarks>
        /// </summary>
        public static void Add<T>( this StringBuilder sb, T item )
        {
            sb.Append( item );
        }
    }
}
