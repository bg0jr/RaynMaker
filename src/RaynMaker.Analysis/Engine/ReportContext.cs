using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Plainion;
using RaynMaker.Analysis.AnalysisSpec;
using RaynMaker.Analysis.AnalysisSpec.Providers;
using RaynMaker.Entities;
using RaynMaker.Entities.Datums;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Analysis.Engine
{
    public class ReportContext : IFigureProviderContext, IExpressionEvaluationContext
    {
        private ICurrenciesLut myCurrenciesLut;
        private List<IFigureProvider> myProviders;
        private List<IFigureProviderFailure> myProviderFailures;

        internal ReportContext( ICurrenciesLut lut, Stock stock, FlowDocument document )
        {
            myCurrenciesLut = lut;
            Stock = stock;
            Document = document;

            var data = new List<IDatumSeries>();
            foreach( var datumType in Dynamics.AllDatums )
            {
                // EnableCurrencyCheck has to be true - otherwise we will not have series.Currency property set
                // except for Price - there we might have different currencies in one collection and currently we anyway
                // just need one price
                data.Add( Dynamics.GetDatumSeries( stock, datumType, datumType != typeof( Price ) ) );
            }
            Data = data;

            myProviders = new List<IFigureProvider>();

            myProviders.Add( new CurrentPrice() );

            foreach( var datumType in Dynamics.AllDatums.Where( t => t != typeof( Price ) ) )
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
            myProviders.Add( new GenericJoinProvider( ProviderNames.DebtEquityRatio, typeof( TotalLiabilities ).Name, typeof( Equity ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericJoinProvider( ProviderNames.InterestCoverage, typeof( EBIT ).Name, typeof( InterestExpense ).Name,
                ( lhs, rhs ) => lhs / rhs ) { PreserveCurrency = false } );
            myProviders.Add( new GenericJoinProvider( ProviderNames.CurrentRatio, typeof( CurrentAssets ).Name, typeof( CurrentLiabilities ).Name,
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

        public Stock Stock { get; private set; }

        public IEnumerable<IDatumSeries> Data { get; private set; }

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

            Contract.Invariant( ( DateTime.Today - translation.Timestamp ).Days < myCurrenciesLut.MaxCurrencyTranslationsAgeInDays,
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
                // TODO: handle duplicates
                myProviderFailures.Add( failure );
                return failure.DefaultValue;
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
