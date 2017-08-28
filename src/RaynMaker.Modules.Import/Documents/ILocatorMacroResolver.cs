using System.Collections.Generic;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Documents
{
    public interface ILocatorMacroResolver
    {
        IEnumerable<string> UnresolvedMacros { get; }

        /// <summary>
        /// Returns uniq ID for the given locator. Implementations have to consider variable parts (e.g. macros)
        /// </summary>
        int CalculateLocationUID( DocumentLocator locator );

        DocumentLocationFragment Resolve( DocumentLocationFragment fragment );
    }
}
