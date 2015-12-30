using System;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    interface INavigator
    {
        event Action<Uri> Navigating;

        Uri Navigate( DocumentLocator navigation );
    }
}
