using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Plainion;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.AnalysisSpec.Providers;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Entities.Datums;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Reporting;
using RaynMaker.Entities;

namespace RaynMaker.Blade.Engine
{
    public class ReportContext : IFigureProviderContext, IExpressionEvaluationContext
    {
        private Project myProject;
        private List<IFigureProvider> myProviders;
        private List<IFigureProviderFailure> myProviderFailures;

        internal ReportContext( Project project, RaynMaker.Blade.Entities.Stock stock, FlowDocument document )
        {
            myProject = project;
            Stock = stock;
            Document = document;

            myProviders = new List<IFigureProvider>();

            myProviders.Add( new CurrentPrice() );

            foreach( var datumType in KnownDatums.AllExceptPrice )
            {
                myProviders.Add( new GenericDatumProvider( datumType ) );
            }

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
            myProviders.Add( new GenericJoinProvider( ProviderNames.DebtEquityRatio, typeof( Debt ).Name, typeof( Equity ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericJoinProvider( ProviderNames.InterestCoverage, typeof( EBIT ).Name, typeof( InterestExpense ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericJoinProvider( ProviderNames.CurrentRatio, typeof( Assets ).Name, typeof( Liabilities ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );

            myProviders.Add( new GenericPriceRatioProvider( ProviderNames.MarketCap, typeof( SharesOutstanding ).Name,
                ( lhs, rhs ) => lhs * rhs ) { PreserveCurrency = true } );
            myProviders.Add( new GenericPriceRatioProvider( ProviderNames.PriceEarningsRatio, ProviderNames.Eps,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericPriceRatioProvider( ProviderNames.PriceToBook, ProviderNames.BookValue,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericPriceRatioProvider( ProviderNames.DividendYield, ProviderNames.DividendPerShare,
                ( lhs, rhs ) => rhs / lhs * 100 ) { PreserveCurrency = false } );

            myProviderFailures = new List<IFigureProviderFailure>();
        }

        public RaynMaker.Blade.Entities.Stock Stock { get; private set; }

        public FlowDocument Document { get; private set; }

        internal string Evaluate( string text )
        {
            var evaluator = new TextEvaluator( new ExpressionEvaluator( this, typeof( Functions ) ) );
            return evaluator.Evaluate( text );
        }

        public object ProvideValue( string expr )
        {
            var evaluator = new TextEvaluator( new ExpressionEvaluator( this, typeof( Functions ) ) );
            return evaluator.ProvideValue( expr );
        }

        public double TranslateCurrency( double value, Currency source, Currency target )
        {
            if( source == null && target == null )
            {
                return value;
            }

            if( source == target )
            {
                return value;
            }

            Contract.RequiresNotNull( source, "source" );
            Contract.RequiresNotNull( target, "target" );

            var translation = source.Translations.SingleOrDefault( t => t.Target == target );

            Contract.Invariant( translation != null, "No translation found from {0} to {1}", source, target );

            Contract.Invariant( ( DateTime.Today - translation.Timestamp ).Days < myProject.MaxCurrencyTranslationsAgeInDays,
                "Translation rate from {0} to {1} expired", source, target );

            return value * translation.Rate;
        }

        public IDatumSeries GetSeries( string name )
        {
            return ( IDatumSeries )ProvideValueInternal( name ) ?? DatumSeries.Empty;
        }

        object IExpressionEvaluationContext.ProvideValue( string name )
        {
            return ProvideValueInternal( name );
        }

        public IEnumerable<IFigureProviderFailure> ProviderFailures { get { return myProviderFailures; } }

        private object ProvideValueInternal( string name )
        {
            var provider = myProviders.SingleOrDefault( p => p.Name == name );
            Contract.Requires( provider != null, "{0} does not represent a IFigureProvider", name );

            var result = provider.ProvideValue( this );

            var failure = result as IFigureProviderFailure;
            if( failure != null )
            {
                myProviderFailures.Add( failure );
                return null;
            }

            return result;
        }

        internal void Complete()
        {
            myProviders = null;

            if( myProviderFailures.Count == 0 )
            {
                return;
            }

            Document.Headline( "Datum provider failures" );
            foreach( var failure in myProviderFailures )
            {
                Document.Paragraph( failure.ToString() );
            }
        }
    }
}
