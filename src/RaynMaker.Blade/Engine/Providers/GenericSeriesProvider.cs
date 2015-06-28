using System;
using System.Linq;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public class GenericSeriesProvider : IFigureProvider
    {
        private Type myDatumType;

        public GenericSeriesProvider( Type datumType )
        {
            myDatumType = datumType;
        }

        public string Name { get { return myDatumType.Name; } }

        public object ProvideValue( Asset asset )
        {
            return asset.Data.OfType<Series>()
                .Where( s => s.Values.Where( v => v.GetType() == myDatumType ).Any() )
                .SingleOrDefault() ?? new Series();
        }
    }
}
