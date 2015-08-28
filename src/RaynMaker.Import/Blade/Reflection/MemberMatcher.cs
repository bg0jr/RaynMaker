using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Reflection
{
    /// <summary />
    public class MemberMatcher
    {
        /// <summary />
        public MethodInfo Method
        {
            get;
            private set;
        }
        /// <summary />
        public MemberMatcher( MethodInfo member )
        {
            this.Method = member;
        }
        /// <summary />
        public bool Matches( MethodInfo mi )
        {
            return this.Method == mi;
        }
        /// <summary />
        public bool Matches( Expression<Func<object>> member )
        {
            return this.Matches( Reflector.GetMethodInfo( member ) );
        }
        /// <summary />
        public TransformAction Become( object value )
        {
            return new TransformAction( this, value );
        }
    }
}
