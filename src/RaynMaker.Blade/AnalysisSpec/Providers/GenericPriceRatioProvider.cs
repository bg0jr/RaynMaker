using System;
using System.Linq;
using Plainion;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Entities.Datums;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
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
            Contract.RequiresNotNullNotEmpty( seriesName, "rhsSeriesName" );
            Contract.RequiresNotNull( CalculateRatio, "Join" );

            mySeriesName = seriesName;
            myRatioCalculationOperator = CalculateRatio;
        }

        public sealed override object ProvideValue( IFigureProviderContext context )
        {
            var price = ( Price )new CurrentPrice().ProvideValue( context );
            if( price == null )
            {
                return new MissingData( "Price" );
            }

            var values = context.GetSeries( mySeriesName );
            if( !values.Any() )
            {
                return new MissingData( mySeriesName );
            }

            Contract.Requires( price.Currency != null, "Currency missing at price" );

            Contract.Requires( values.Currency == null || price.Currency == values.Currency,
                "Currency inconsistencies detected: Price.Currency={0} vs {1}.Currency={2}",
                price.Currency, values.Name, values.Currency );

            var priceYear = price.Period.Year();

            var value = values.SingleOrDefault( e => e.Period.Year() == priceYear );
            if( value == null )
            {
                value = values.SingleOrDefault( e => e.Period.Year() == priceYear - 1 );
                if( value == null )
                {
                    return new MissingDataForPeriod( mySeriesName, new YearPeriod( priceYear ), new YearPeriod( priceYear - 1 ) );
                }
            }

            var result = new DerivedDatum
            {
                Period = price.Period,
                Value = myRatioCalculationOperator( price.Value.Value, value.Value.Value )
            };

            if( PreserveCurrency )
            {
                result.Currency = price.Currency;
            }

            result.Inputs.Add( price );
            result.Inputs.Add( value );

            return result;
        }
    }
}
