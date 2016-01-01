using System;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Locating
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "Request" )]
    public class Request : DocumentLocationFragment
    {
        public Request( Uri url )
            : base( url )
        {
        }

        public Request( string url )
            : base( url )
        {
        }
    }
}
