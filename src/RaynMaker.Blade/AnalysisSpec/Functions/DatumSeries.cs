using System;
using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Functions
{
    public class DatumSeries : IFigureProvider
    {
        private Type myDatumType;

        public DatumSeries( Type datumType )
        {
            myDatumType = datumType;
        }

        public string Name { get { return myDatumType.Name; } }

        public object ProvideValue( IFigureProviderContext context)
        {
            return context.Asset.Data.OfType<Series>()
                .Where( s => s.Values.Where( v => v.GetType() == myDatumType ).Any() )
                .SingleOrDefault() ?? new Series();
        }
    }
}
