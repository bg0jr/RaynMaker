using System;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public interface INavigator
    {
        Uri Navigate( Navigation navigation );
    }
}
