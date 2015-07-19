using System.Linq;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Functions
{
    public class DividendYield : IFigureProvider
    {
        public string Name { get { return FunctionNames.DividendYield; } }

        public object ProvideValue( IFigureProviderContext context )
        {
            var price = context.Asset.Data.OfType<Price>().SingleOrDefault();
            if( price == null )
            {
                return null;
            }

            var dividends = context.GetDatumSeries<Dividend>();

            var dividend = dividends.SingleOrDefault( d => d.Year == price.Date.Year );
            if( dividend == null )
            {
                dividend = dividends.SingleOrDefault( d => d.Year == price.Date.Year - 1 );
                if( dividend == null )
                {
                    return null;
                }
            }

            Contract.Requires( price.Currency == dividend.Currency, "Currency mismatch" );

            var result = new DerivedDatum
            {
                Date = price.Date,
                Value = dividend.Value / price.Value * 100
            };
            result.Inputs.Add( dividend );
            result.Inputs.Add( price );
            return result;
        }
    }
}
