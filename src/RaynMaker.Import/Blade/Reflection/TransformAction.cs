using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Reflection
{
    /// <summary />
    public class TransformAction
    {
        /// <summary />
        public MemberMatcher Matcher
        {
            get;
            private set;
        }
        /// <summary />
        public object Value
        {
            get;
            private set;
        }
        /// <summary />
        public TransformAction( MemberMatcher matcher, object value )
        {
            this.Matcher = matcher;
            this.Value = value;
        }
    }
}
