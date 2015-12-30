using System;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import
{
    interface INavigator
    {
        event Action<Uri> Navigating;

        Uri Navigate( DocumentLocator navigation );
    }
}
