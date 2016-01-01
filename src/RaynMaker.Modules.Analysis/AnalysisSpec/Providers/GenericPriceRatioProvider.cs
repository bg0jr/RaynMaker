using System;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Modules.Analysis.AnalysisSpec.Providers
{
    /// <summary>
    /// Returns a ratio of a given datum to the current price
    /// </summary>
    public class GenericPriceRatioProvider : AbstractProvider
    {
        private string mySeriesName;
        private Func<double, double, double> myRatioCalculationOperator;

        public GenericPriceRatioProvider( string name, string seriesName, Func<double, double, double> CalculateRatio )
            : base( name )
        {
            Contract.RequiresNotNullNotEmpty( seriesName, "seriesName" );
            Contract.RequiresNotNull( CalculateRatio, "CalculateRatio" );

            mySeriesName = seriesName;
            myRatioCalculationOperator = CalculateRatio;
        }

        public sealed override object ProvideValue( IFigureProviderContext context )
        {
            var price = ( Price )context.Data.SeriesOf( typeof( Price ) ).Current();
            if( price == null )
            {
                return new MissingData( "Price", null );
            }

            var values = context.GetSeries( mySeriesName );
            if( !values.Any() )
            {
                return new MissingData( mySeriesName, null );
            }

            Contract.Requires( price.Currency != null, "Currency missing at price" );

            var priceValue = price.Value.Value;
            if( values.Currency != null && price.Currency != values.Currency )
            {
                priceValue = context.TranslateCurrency( price.Value.Value, price.Currency, values.Currency );
            }

            var priceYear = price.Period.Year();

            var value = values.SingleOrDefault( e => e.Period.Year() == priceYear );
            if( value == null )
            {
                value = values.SingleOrDefault( e => e.Period.Year() == priceYear - 1 );
                if( value == null )
                {
                    return new MissingDataForPeriod( mySeriesName, null, new YearPeriod( priceYear ), new YearPeriod( priceYear - 1 ) );
                }
            }

            var result = new DerivedDatum
            {
                Period = price.Period,
                Value = myRatioCalculationOperator( priceValue, value.Value.Value )
            };

            if( PreserveCurrency )
            {
                result.Currency = values.Currency != null ? values.Currency : price.Currency;
            }

            result.Inputs.Add( price );
            result.Inputs.Add( value );

            return result;
        }
    }
}
