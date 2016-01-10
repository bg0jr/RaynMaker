using System.Collections.Generic;
using System.Linq;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Documents
{
    public class NullLocatorMacroResolver : ILocatorMacroResolver
    {
        public DocumentLocationFragment Resolve( DocumentLocationFragment fragment )
        {
            return fragment;
        }

        public IEnumerable<string> UnresolvedMacros
        {
            get { return Enumerable.Empty<string>(); }
        }
    }
}
