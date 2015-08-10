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
            var allLhs = context.GetSeries<IAnualDatum>( myLhsSeriesName );
            if( !allLhs.Any() )
            {
                AddFailureReason( "Missing input for {0}", myLhsSeriesName );
            }

            var allRhs = context.GetSeries<IAnualDatum>( myRhsSeriesName );
            if( !allLhs.Any() )
            {
                AddFailureReason( "Missing input for {0}", myRhsSeriesName );
            }

            if( FailureReasons.Any() )
            {
                return new Series();
            }

            var resultSeries = new Series();

            var lhs = allLhs
                .OrderByDescending( a => a.Year )
                .First();

            var rhs = allRhs.SingleOrDefault( d => d.Year == lhs.Year );
            if( rhs == null )
            {
                AddFailureReason( "No '{0}' for year {1} found", myRhsSeriesName, lhs.Year );
                return null;
            }

            var result = new DerivedDatum
            {
                Year = lhs.Year,
                Value = myRatioCalculationOperator( lhs.Value, rhs.Value )
            };

            if( PreserveCurrency )
            {
                var lhsCurrency = lhs as ICurrencyDatum;
                var rhsCurrency = rhs as ICurrencyDatum;
                if( lhsCurrency != null )
                {
                    result.Currency = lhsCurrency.Currency;

                    if( rhsCurrency != null )
                    {
                        Contract.Requires( lhsCurrency.Currency != rhsCurrency.Currency,
                            "Currency inconsistencies found: {0} vs {1}", lhsCurrency.Currency, rhsCurrency.Currency );
                    }
                }
                else if( rhsCurrency != null )
                {
                    result.Currency = rhsCurrency.Currency;
                }
            }

            result.Inputs.Add( lhs );
            result.Inputs.Add( rhs );
            return result;
        }
    }
}
