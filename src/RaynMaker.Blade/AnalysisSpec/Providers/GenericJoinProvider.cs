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
            var allLhs = context.GetSeries<IDatum>( myLhsSeriesName );
            if( !allLhs.Any() )
            {
                AddFailureReason( "Missing input for {0}", myLhsSeriesName );
            }

            var allRhs = context.GetSeries<IDatum>( myRhsSeriesName );
            if( !allLhs.Any() )
            {
                AddFailureReason( "Missing input for {0}", myRhsSeriesName );
            }

            if( FailureReasons.Any() )
            {
                return new Series();
            }

            var resultSeries = new Series();

            bool isAnual = allLhs.OfType<IAnualDatum>().Any();

            foreach( var lhs in allLhs )
            {
                var rhs = allRhs.SingleOrDefault( e => isAnual ?
                    ( ( IAnualDatum )e ).Year == ( ( IAnualDatum )lhs ).Year :
                    ( ( IDailyDatum )e ).Date == ( ( IDailyDatum )lhs ).Date );

                if( rhs == null )
                {
                    continue;
                }

                var result = new DerivedDatum
                {
                    Value = myJoinOperator( lhs.Value, rhs.Value )
                };

                if( isAnual )
                {
                    result.Year = ( ( IAnualDatum )lhs ).Year;
                }
                else
                {
                    result.Date = ( ( IDailyDatum )lhs ).Date;
                }

                // TODO: check for currency consistancy (if one has currency, others must have as well)

                var currencyDatum = lhs as ICurrencyDatum;
                if( currencyDatum != null )
                {
                    result.Currency = currencyDatum.Currency;
                }

                result.Inputs.Add( lhs );
                result.Inputs.Add( rhs );

                resultSeries.Values.Add( result );
            }

            return resultSeries;
        }
    }
}
