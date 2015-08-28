using System;
using System.Linq;
using RaynMaker.Analysis.Engine;
using RaynMaker.Entities;

namespace RaynMaker.Analysis.AnalysisSpec.Providers
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
            return context.Data.OfType<IDatumSeries>()
                .Where( s => s.DatumType == myDatumType )
                .SingleOrDefault() ?? DatumSeries.Empty;
        }
    }
}
