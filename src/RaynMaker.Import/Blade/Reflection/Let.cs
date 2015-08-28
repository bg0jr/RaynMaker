using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Reflection
{
    /// <summary />
    public static class Let
    {
        /// <summary />
        public static MemberMatcher Member( Expression<Func<object>> member )
        {
            return new MemberMatcher( Reflector.GetMethodInfo( member ) );
        }
    }
}
