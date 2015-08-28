using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Collections
{
    /// <summary />
    public static class Set
    {
        /// <summary>
        /// Converts the given elements into an ienumerable.
        /// </summary>
        public static IEnumerable<T> ToSet<T>( params T[] elements )
        {
            return elements;
        }
        /// <summary>
        /// Converts the given elements int an array.
        /// </summary>
        public static T[] ToArray<T>( params T[] elements )
        {
            return elements;
        }
    }
}
