using System;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public interface INavigator
    {
        event Action<Uri> Navigating;

        Uri Navigate( Navigation navigation );
    }
}
