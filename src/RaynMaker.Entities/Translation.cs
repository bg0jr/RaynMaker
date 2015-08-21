using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RaynMaker.Entities
{
    [DataContract( Name = "Translation", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Currency ) )]
    public class Translation : SerializableBindableBase
    {
        private DateTime myTimestamp;
        private Currency myTarget;
        private double myRate;

        [DataMember( Name = "Target" )]
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

        [DataMember]
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

        [DataMember]
        [Required]
        public DateTime Timestamp
        {
            get { return myTimestamp; }
            private set { SetProperty( ref myTimestamp, value ); }
        }
    }
}
