using System.Collections.Generic;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Engine
{
    static class FigureProviderContextExtensions
    {
        public static IDatumSeries GetDatumSeries<T>( this IFigureProviderContext self )
        {
            return self.GetSeries( typeof( T ).Name );
        }
    }
}
