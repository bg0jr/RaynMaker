using System;
using System.Runtime.Serialization;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "Translation", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Currency ) )]
    public class Translation
    {
        private Currency myTarget;

        [DataMember]
        public string To { get; set; }

        public Currency Target
        {
            get
            {
                if( myTarget == null )
                {
                    myTarget = Currencies.Parse( To );
                }

                return myTarget;
            }
        }

        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public double Rate { get; set; }
    }
}
