using System;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Locating
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "Response" )]
    public class Response : DocumentLocationFragment
    {
        public Response( Uri url )
            : base( url )
        {
        }

        public Response( string url )
            : base( url )
        {
        }
    }
}
