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

        public object ProvideValue( IFigureProviderContext context )
        {
            return context.Asset.Data.OfType<IDatumSeries>()
                .Where( s => s.DatumType == myDatumType )
                .SingleOrDefault() ?? Series.Empty;
        }
    }
}
