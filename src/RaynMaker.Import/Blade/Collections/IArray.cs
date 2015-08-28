using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Collections
{
    /// <summary>
    /// Immutable list of elements.
    /// </summary>
    public interface IArray<T> : IEnumerable<T>, IEnumerable
    {
        /// <summary />
        int Count
        {
            get;
        }
        /// <summary />
        T this[ int index ]
        {
            get;
        }
        /// <summary />
        bool Contains( T item );
        /// <summary />
        int IndexOf( T item );
    }
}
