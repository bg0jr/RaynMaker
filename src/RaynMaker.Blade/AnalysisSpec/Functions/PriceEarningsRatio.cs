using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Functions
{
    public class PriceEarningsRatio : IFigureProvider
    {
        public string Name { get { return FunctionNames.PriceEarningsRatio; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var price = context.Asset.Data.OfType<Price>().SingleOrDefault();
            if( price == null )
            {
                return null;
            }

            var allEps = context.GetCalculatedSeries<IAnualFinancialDatum>( FunctionNames.Eps );

            var eps = allEps.SingleOrDefault( d => d.Year == price.Date.Year );
            if( eps == null )
            {
                eps = allEps.SingleOrDefault( d => d.Year == price.Date.Year - 1 );
                if( eps == null )
                {
                    return null;
                }
            }

            var result = new DerivedDatum
            {
                Date = price.Date,
                Value = price.Value / eps.Value
            };
            result.Inputs.Add( eps );
            result.Inputs.Add( price );
            return result;
        }
    }
}
