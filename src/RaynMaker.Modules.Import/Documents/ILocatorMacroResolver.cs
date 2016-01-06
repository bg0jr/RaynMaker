using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Documents
{
    public interface ILocatorMacroResolver
    {
        DocumentLocationFragment Resolve( DocumentLocationFragment fragment );
    }
}
