using System.Linq;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public class PriceToBook : IFigureProvider
    {
        public string Name { get { return ProviderNames.PriceToBook; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var price = context.Asset.Data.OfType<Price>().SingleOrDefault();
            if( price == null )
            {
                return null;
            }

            var allBookValues = context.GetCalculatedSeries<IAnualFinancialDatum>( ProviderNames.BookValue );

            var bookValue = allBookValues.SingleOrDefault( d => d.Year == price.Date.Year );
            if( bookValue == null )
            {
                bookValue = allBookValues.SingleOrDefault( d => d.Year == price.Date.Year - 1 );
                if( bookValue == null )
                {
                    return null;
                }
            }

            var result = new DerivedDatum
            {
                Date = price.Date,
                Value = price.Value / bookValue.Value
            };
            result.Inputs.Add( bookValue );
            result.Inputs.Add( price );
            return result;
        }
    }
}
