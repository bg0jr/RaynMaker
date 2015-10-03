using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.Practices.Prism.Mvvm;

namespace RaynMaker.Entities
{
    public abstract class EntityTimestampBase : EntityBase
    {
        private DateTime myTimestamp;

        internal bool IsMaterialized { get; set; }

        [Required]
        public DateTime Timestamp
        {
            get { return myTimestamp; }
            private set { SetProperty( ref myTimestamp, value ); }
        }

        protected void UpdateTimestamp()
        {
            if( Id == 0 || IsMaterialized )
            {
                Timestamp = DateTime.Now;
            }
        }
    }
}
