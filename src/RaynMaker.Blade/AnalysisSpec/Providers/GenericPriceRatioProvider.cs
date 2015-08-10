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

            bool isAnual = values.OfType<IAnualDatum>().Any();

            var value = values.SingleOrDefault( e => isAnual ?
                ( ( IAnualDatum )e ).Year == price.Date.Year :
                ( ( IDailyDatum )e ).Date.Year == price.Date.Year );
            if( value == null )
            {
                value = values.SingleOrDefault( e => isAnual ?
                   ( ( IAnualDatum )e ).Year == price.Date.Year - 1 :
                   ( ( IDailyDatum )e ).Date.Year == price.Date.Year - 1 );
                if( value == null )
                {
                    AddFailureReason( "No '{0}' for year {1} or {2} found", mySeriesName, price.Date.Year, price.Date.Year - 1 );
                    return null;
                }
            }

            var result = new DerivedDatum
            {
                Date = price.Date,
                Value = price.Value / value.Value
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
