using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public interface INavigator
    {
        Uri Navigate( Navigation navigation );
    }
}
