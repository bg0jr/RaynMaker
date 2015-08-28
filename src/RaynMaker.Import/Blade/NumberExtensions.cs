using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade
{
    /// <summary>
    /// Extensions for numeric values to increase usability.
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Supports Ruby like syntax for a number or repetitions
        /// (for loops).
        /// <example>
        /// <code>
        /// 5.Times( i =&gt; Console.WriteLine( "." ) );
        /// </code>
        /// </example>
        /// </summary>
        public static void Times( this int count, Action<int> action )
        {
            for( int i = 0; i < count; i++ )
            {
                action( i );
            }
        }
    }
}
