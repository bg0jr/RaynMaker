using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public interface IDatumProviderFactory
    {
        DatumLocatorRepository LocatorRepository { get; }
        IDatumProvider Create( string datum );
        IDatumProvider Create( DatumLocator datumLocator );
    }
}
