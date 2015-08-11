using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Plainion;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.AnalysisSpec.Providers;
using RaynMaker.Blade.DataSheetSpec;
using RaynMaker.Blade.DataSheetSpec.Datums;

namespace RaynMaker.Blade.Engine
{
    public class ReportContext : IFigureProviderContext
    {
        private List<IFigureProvider> myProviders;

        public ReportContext( Asset asset, FlowDocument document )
        {
            Asset = asset;
            Document = document;

            myProviders = new List<IFigureProvider>();

            myProviders.Add( new CurrentPrice() );

            myProviders.Add( new GenericDatumProvider( typeof( SharesOutstanding ) ) );
            myProviders.Add( new GenericDatumProvider( typeof( NetIncome ) ) );
            myProviders.Add( new GenericDatumProvider( typeof( Equity ) ) );
            myProviders.Add( new GenericDatumProvider( typeof( Dividend ) ) );
            myProviders.Add( new GenericDatumProvider( typeof( Assets ) ) );
            myProviders.Add( new GenericDatumProvider( typeof( Liabilities ) ) );
            myProviders.Add( new GenericDatumProvider( typeof( Dept ) ) );
            myProviders.Add( new GenericDatumProvider( typeof( Revenue ) ) );
            myProviders.Add( new GenericDatumProvider( typeof( EBIT ) ) );
            myProviders.Add( new GenericDatumProvider( typeof( InterestExpense ) ) );

            myProviders.Add( new GenericJoinProvider( ProviderNames.Eps, typeof( NetIncome ).Name, typeof( SharesOutstanding ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = true } );
            myProviders.Add( new GenericJoinProvider( ProviderNames.BookValue, typeof( Equity ).Name, typeof( SharesOutstanding ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = true } );
            myProviders.Add( new GenericJoinProvider( ProviderNames.DividendPayoutRatio, typeof( Dividend ).Name, typeof( NetIncome ).Name,
                ( lhs, rhs ) => lhs / rhs * 100 ) { PreserveCurrency = false } );
            myProviders.Add( new GenericJoinProvider( ProviderNames.ReturnOnEquity, typeof( NetIncome ).Name, typeof( Equity ).Name,
                ( lhs, rhs ) => lhs / rhs * 100 ) { PreserveCurrency = false } );
            myProviders.Add( new GenericJoinProvider( ProviderNames.DividendPerShare, typeof( Dividend ).Name, typeof( SharesOutstanding ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = true } );

            myProviders.Add( new GenericPriceRatioProvider( ProviderNames.MarketCap, typeof( SharesOutstanding ).Name,
                ( lhs, rhs ) => lhs * rhs ) { PreserveCurrency = true } );
            myProviders.Add( new GenericPriceRatioProvider( ProviderNames.PriceEarningsRatio, ProviderNames.Eps,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericPriceRatioProvider( ProviderNames.PriceToBook, ProviderNames.BookValue,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericPriceRatioProvider( ProviderNames.DividendYield, ProviderNames.DividendPerShare,
                ( lhs, rhs ) => rhs / lhs * 100 ) { PreserveCurrency = false } );

            myProviders.Add( new GenericCurrentRatioProvider( ProviderNames.DeptEquityRatio, typeof( Dept ).Name, typeof( Equity ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericCurrentRatioProvider( ProviderNames.InterestCoverage, typeof( EBIT ).Name, typeof( InterestExpense ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericCurrentRatioProvider( ProviderNames.CurrentRatio, typeof( Assets ).Name, typeof( Liabilities ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
        }

        public Asset Asset { get; private set; }

        public FlowDocument Document { get; private set; }

        internal string Evaluate( string text )
        {
            var evaluator = new TextEvaluator( new ExpressionEvaluator( myProviders, this, typeof( Functions ) ) );
            return evaluator.Evaluate( text );
        }

        public object ProvideValue( string expr )
        {
            var evaluator = new TextEvaluator( new ExpressionEvaluator( myProviders, this, typeof( Functions ) ) );
            return evaluator.ProvideValue( expr );
        }

        public double TranslateCurrency( double value, Currency source, Currency target )
        {
            if( source == null && target == null )
            {
                return value;
            }

            Contract.RequiresNotNull( source, "source" );
            Contract.RequiresNotNull( target, "target" );

            var translation = source.Translations.SingleOrDefault( t => t.Target == target );

            Contract.Invariant( translation != null, "No translation found from {0} to {1}", source, target );

            Contract.Invariant( ( DateTime.Today - translation.Timestamp ).Days < Currencies.Sheet.MaxAgeInDays,
                "Translation rate from {0} to {1} expired", source, target );

            return value * translation.Rate;
        }

        public IDatumSeries GetSeries( string name )
        {
            var provider = myProviders.SingleOrDefault( p => p.Name == name );
            Contract.RequiresNotNull( provider, "No provider found with name: " + name );

            return ( IDatumSeries)provider.ProvideValue( this );
        }
    }
}
