using RaynMaker.Analysis.Engine;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Analysis.AnalysisSpec.Providers
{
    public class CurrentPrice : IFigureProvider
    {
        public string Name { get { return ProviderNames.CurrentPrice; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var price = context.Data.SeriesOf( typeof( Price ) ).Current();
            if( price == null )
            {
                return new MissingData( "Price", null );
            }
            return price;
        }
    }
}
