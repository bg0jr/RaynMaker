using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RaynMaker.Entities
{
    public class Translation : EntityBase
    {
        private DateTime myTimestamp;
        private Currency mySource;
        private Currency myTarget;
        private double myRate;

        [Required]
        public long Id { get; set; }

        public long SourceId { get; set; }

        [Required]
        public Currency Source
        {
            get { return mySource; }
            set
            {
                if( SetProperty( ref mySource, value ) )
                {
                    Timestamp = DateTime.Now;
                }
            }
        }

        public long TargetId { get; set; }
        
        [Required]
        public Currency Target
        {
            get { return myTarget; }
            set
            {
                if( SetProperty( ref myTarget, value ) )
                {
                    Timestamp = DateTime.Now;
                }
            }
        }

        [Required]
        public double Rate
        {
            get { return myRate; }
            set
            {
                if( SetProperty( ref myRate, value ) )
                {
                    Timestamp = DateTime.Now;
                }
            }
        }

        [Required]
        public DateTime Timestamp
        {
            get { return myTimestamp; }
            private set { SetProperty( ref myTimestamp, value ); }
        }
    }
}
