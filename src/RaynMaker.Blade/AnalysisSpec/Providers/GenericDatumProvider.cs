using System;
using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public class GenericDatumProvider : IFigureProvider
    {
        private Type myDatumType;

        public GenericDatumProvider( Type datumType )
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
