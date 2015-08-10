using System.Collections.Generic;

namespace RaynMaker.Blade.Engine
{
    static class FigureProviderContextExtensions
    {
        public static IEnumerable<T> GetDatumSeries<T>( this IFigureProviderContext self )
        {
            return self.GetSeries<T>( typeof( T ).Name );
        }
    }
}
