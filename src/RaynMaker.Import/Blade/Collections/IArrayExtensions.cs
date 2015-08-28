using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
