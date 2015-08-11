using System;
using System.Linq;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;
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
            var price = context.Asset.Data.OfType<Price>().SingleOrDefault();
            if( price == null )
            {
                AddFailureReason( "No Price found" );
                return null;
            }

            var values = context.GetSeries<IDatum>( mySeriesName );
            if( !values.Any() )
            {
                AddFailureReason( "Missing input for {0}", mySeriesName );
                return null;
            }

            var priceYear = price.Period.Year();

            // TODO: how to specify "include?" 
            // TODO: how to specify "one previous period"
            var value = values.SingleOrDefault( e => e.Period.Year() == priceYear );
            if( value == null )
            {
                value = values.SingleOrDefault( e => e.Period.Year() == priceYear - 1 );
                if( value == null )
                {
                    AddFailureReason( "No '{0}' for year {1} or {2} found", mySeriesName, priceYear, priceYear - 1 );
                    return null;
                }
            }

            var result = new DerivedDatum
            {
                Period = price.Period,
                Value = myRatioCalculationOperator( price.Value, value.Value )
            };

            if( PreserveCurrency )
            {
                result.Currency = price.Currency;
            }

            result.Inputs.Add( value );
            result.Inputs.Add( price );

            return result;
        }
    }
}
