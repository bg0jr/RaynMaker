using System;
using System.Linq;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    /// <summary>
    /// Joins two input series using the given operator.
    /// </summary>
    public sealed class GenericJoinProvider : AbstractProvider
    {
        private string myLhsSeriesName;
        private string myRhsSeriesName;
        private Func<double, double, double> myJoinOperator;

        public GenericJoinProvider( string name, string lhsSeriesName, string rhsSeriesName, Func<double, double, double> Join )
            : base( name )
        {
            Contract.RequiresNotNullNotEmpty( lhsSeriesName, "lhsSeriesName" );
            Contract.RequiresNotNullNotEmpty( rhsSeriesName, "rhsSeriesName" );
            Contract.RequiresNotNull( Join, "Join" );

            myLhsSeriesName = lhsSeriesName;
            myRhsSeriesName = rhsSeriesName;
            myJoinOperator = Join;
        }

        public sealed override object ProvideValue( IFigureProviderContext context )
        {
            var allLhs = context.GetSeries( myLhsSeriesName );
            if( !allLhs.Any() )
            {
                return new MissingData( myLhsSeriesName );
            }

            Contract.Requires( allLhs.IsFrozen, "Series not frozen: {0}", myLhsSeriesName );
            
            var allRhs = context.GetSeries( myRhsSeriesName );
            if( !allRhs.Any() )
            {
                return new MissingData( myRhsSeriesName );
            }

            Contract.Requires( allRhs.IsFrozen, "Series not frozen: {0}", myRhsSeriesName );
            
            EnsureCurrencyConsistancy( allLhs, allRhs );
            
            var resultSeries = new Series();

            foreach( var lhs in allLhs )
            {
                var rhs = allRhs.SingleOrDefault( e => e.Period.Equals( lhs.Period ) );

                if( rhs == null )
                {
                    continue;
                }

                var result = new DerivedDatum
                {
                    Value = myJoinOperator( lhs.Value, rhs.Value ),
                    Period = lhs.Period
                };

                if( PreserveCurrency )
                {
                    result.Currency = allLhs.Currency ?? allRhs.Currency;
                }

                result.Inputs.Add( lhs );
                result.Inputs.Add( rhs );

                resultSeries.Values.Add( result );
            }

            resultSeries.Freeze();

            return resultSeries;
        }
    }
}
