using System.Collections.Generic;
using System.Runtime.Serialization;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.Services
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DatumLocatorSheet" )]
    class DatumLocatorSheet
    {
        [DataMember]
        public IEnumerable<DatumLocator> Locators { get; set; }
    }
}
