using System.Collections.Generic;

namespace RaynMaker.Import.Spec
{
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

        public string Datum { get; private set; }

        public IList<Site> Sites { get; private set; }
    }
}
