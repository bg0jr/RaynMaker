using System;
using System.Linq;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    /// <summary>
    /// Returns a ratio of given input series for the most recent period
    /// </summary>
    public sealed class GenericCurrentRatioProvider : AbstractProvider
    {
        private string myLhsSeriesName;
        private string myRhsSeriesName;
        private Func<double, double, double> myRatioCalculationOperator;

        public GenericCurrentRatioProvider( string name, string lhsSeriesName, string rhsSeriesName, Func<double, double, double> CalculateRatio )
            : base( name )
        {
            Contract.RequiresNotNullNotEmpty( lhsSeriesName, "lhsSeriesName" );
            Contract.RequiresNotNullNotEmpty( rhsSeriesName, "rhsSeriesName" );
            Contract.RequiresNotNull( CalculateRatio, "CalculateRatio" );

            myLhsSeriesName = lhsSeriesName;
            myRhsSeriesName = rhsSeriesName;
            myRatioCalculationOperator = CalculateRatio;
        }

        public sealed override object ProvideValue( IFigureProviderContext context )
        {
            var allLhs = context.GetSeries( myLhsSeriesName );
            if( !allLhs.Any() )
            {
                return new MissingData( myLhsSeriesName );
            }

            var allRhs = context.GetSeries( myRhsSeriesName );
            if( !allRhs.Any() )
            {
                return new MissingData( myRhsSeriesName );
            }

            EnsureCurrencyConsistancy( allLhs, allRhs );

            var lhs = allLhs
                .OrderByDescending( a => a.Period )
                .First();

            var rhs = allRhs.SingleOrDefault( d => d.Period.Equals( lhs.Period ) );
            if( rhs == null )
            {
                return new MissingDataForPeriod( myRhsSeriesName, lhs.Period );
            }

            var result = new DerivedDatum
            {
                Period = lhs.Period,
                Value = myRatioCalculationOperator( lhs.Value, rhs.Value )
            };

            if( PreserveCurrency )
            {
                result.Currency = allLhs.Currency ?? allRhs.Currency;
            }

            result.Inputs.Add( lhs );
            result.Inputs.Add( rhs );

            return result;
        }
    }
}
