using System.Collections.Generic;
using Blade.Reflection;

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

        public DatumLocator( DatumLocator provider, params TransformAction[] rules )
        {
            Datum = rules.ApplyTo<string>( () => provider.Datum );
            Sites = rules.ApplyTo<IList<Site>>( () => provider.Sites );
        }
       
        public string Datum { get; private set; }

        public IList<Site> Sites { get; private set; }
    }
}
