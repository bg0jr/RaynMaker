using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade
{
    /// <summary />
    public class ManagedObject : IDisposable
    {
        /// <summary />
        protected bool IsDisposed
        {
            get;
            private set;
        }
        /// <summary />
        public ManagedObject()
        {
            this.IsDisposed = false;
        }
        ~ManagedObject()
        {
            this.Dispose( false );
        }
        /// <summary />
        public void Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }
        /// <summary />
        protected virtual void Dispose( bool disposing )
        {
            this.IsDisposed = true;
        }
    }
}
