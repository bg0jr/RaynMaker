using System;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import
{
    interface INavigator
    {
        event Action<Uri> Navigating;

        Uri Navigate( DocumentLocator navigation, ILocatorMacroResolver macroResolver );
    }
}
