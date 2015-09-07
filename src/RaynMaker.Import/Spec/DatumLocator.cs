using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DatumLocator" )]
    public class DatumLocator
    {
        public DatumLocator( string datum, params Site[] sites )
            : this( datum, new List<Site>( sites ) )
        {
        }

        public DatumLocator( string datum, IList<Site> sites )
        {
            Datum = datum;
            Sites = sites;
        }

        [DataMember]
        public string Datum { get; private set; }

        [DataMember]
        public IList<Site> Sites { get; private set; }
    }
}
