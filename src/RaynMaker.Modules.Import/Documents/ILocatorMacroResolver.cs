using System.Collections.Generic;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Documents
{
    public interface ILocatorMacroResolver
    {
        IEnumerable<string> UnresolvedMacros { get; }

        int CalculateLocationUID( DocumentLocator locator );

        DocumentLocationFragment Resolve( DocumentLocationFragment fragment );
    }
}
