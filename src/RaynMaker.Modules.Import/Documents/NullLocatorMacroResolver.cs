using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Documents
{
    public class NullLocatorMacroResolver : ILocatorMacroResolver
    {
        public DocumentLocationFragment Resolve( DocumentLocationFragment fragment )
        {
            return fragment;
        }
    }
}
