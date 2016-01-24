using System;
using System.Linq;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Analysis.AnalysisSpec.Providers
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
            var series = context.Data.OfType<IFigureSeries>()
                .Where( s => s.FigureType == myDatumType )
                .SingleOrDefault();

            if( series == null )
            {
                return new MissingData( Name, FigureSeries.Empty );
            }

            return series;
        }
    }
}
