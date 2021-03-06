﻿using System;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Analysis.Engine;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Analysis.AnalysisSpec.Providers
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
                return new MissingData( myLhsSeriesName, FigureSeries.Empty );
            }

            var allRhs = context.GetSeries( myRhsSeriesName );
            if( !allRhs.Any() )
            {
                return new MissingData( myRhsSeriesName, FigureSeries.Empty );
            }

            EnsureCurrencyConsistancy( allLhs, allRhs );

            var resultSeries = new FigureSeries( typeof( DerivedFigure ) );

            foreach( var lhs in allLhs )
            {
                var rhs = allRhs.SingleOrDefault( e => e.Period.Equals( lhs.Period ) );

                if( rhs == null )
                {
                    continue;
                }

                var result = new DerivedFigure
                {
                    Value = myJoinOperator( lhs.Value.Value, rhs.Value.Value ),
                    Period = lhs.Period
                };

                if( PreserveCurrency )
                {
                    result.Currency = allLhs.Currency ?? allRhs.Currency;
                }

                result.Inputs.Add( lhs );
                result.Inputs.Add( rhs );

                resultSeries.Add( result );
            }

            return resultSeries;
        }
    }
}
