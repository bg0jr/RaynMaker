using System;
using System.Linq;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Analysis.AnalysisSpec.Providers
{
    public class GenericFigureProvider : IFigureProvider
    {
        private Type myFigureType;

        public GenericFigureProvider( Type figureType )
        {
            myFigureType = figureType;
        }

        public string Name { get { return myFigureType.Name; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var series = context.Data.OfType<IFigureSeries>()
                .Where( s => s.FigureType == myFigureType )
                .SingleOrDefault();

            if( series == null )
            {
                return new MissingData( Name, FigureSeries.Empty );
            }

            return series;
        }
    }
}
