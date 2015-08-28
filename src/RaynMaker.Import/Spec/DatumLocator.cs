using System.Collections.Generic;
using Blade;
using Blade.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace RaynMaker.Import.Spec
{
    public class DatumLocator : INamedObject
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

        /// <summary>
        /// Actually returns the datum.
        /// </summary>
        public string Name { get { return Datum; } }
       
        public string Datum { get; private set; }

        public IList<Site> Sites { get; private set; }
    }
}
