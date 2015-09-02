using System.Collections.Generic;

namespace Blade.Collections
{
    /// <summary />
    public static class IArrayExtensions
    {
        /// <summary />
        public static IArray<T> ToArrayList<T>( this IEnumerable<T> set )
        {
            return new ActiveList<T>( set );
        }
    }
}
