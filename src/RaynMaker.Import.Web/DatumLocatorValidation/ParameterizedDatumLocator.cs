using System.Collections.Generic;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.DatumLocatorValidation
{
    public class ParameterizedDatumLocator
    {
        public ParameterizedDatumLocator( string datum, Site site )
        {
            Datum = datum;
            Site = site;

            Name = string.Format( "{0}: {1}", datum, site.Name );
            Parameters = new Dictionary<string, string>();
        }

        public string Datum { get; private set; }

        public string Name { get; private set; }

        public Site Site { get; private set; }

        public IDictionary<string, string> Parameters { get; private set; }
    }
}
