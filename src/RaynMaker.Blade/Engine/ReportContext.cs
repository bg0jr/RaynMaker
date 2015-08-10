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

            myProviders = GetType().Assembly.GetTypes()
                .Where( t => t.GetInterfaces().Any( iface => iface == typeof( IFigureProvider ) ) )
                .Where( t => !t.IsAbstract )
                .Where( t => t != typeof( DatumSeries ) )
                .Where( t => t != typeof( GenericJoinProvider ) )
                .Select( t => Activator.CreateInstance( t ) )
                .OfType<IFigureProvider>()
                .ToList();

            myProviders.Add( new DatumSeries( typeof( SharesOutstanding ) ) );
            myProviders.Add( new DatumSeries( typeof( NetIncome ) ) );
            myProviders.Add( new DatumSeries( typeof( Equity ) ) );
            myProviders.Add( new DatumSeries( typeof( Dividend ) ) );
            myProviders.Add( new DatumSeries( typeof( Assets ) ) );
            myProviders.Add( new DatumSeries( typeof( Liabilities ) ) );
            myProviders.Add( new DatumSeries( typeof( Dept ) ) );
            myProviders.Add( new DatumSeries( typeof( Revenue ) ) );
            myProviders.Add( new DatumSeries( typeof( EBIT ) ) );
            myProviders.Add( new DatumSeries( typeof( InterestExpense ) ) );

            myProviders.Add( new GenericJoinProvider( ProviderNames.Eps, typeof( NetIncome ).Name, typeof( SharesOutstanding ).Name, 
                ( lhs, rhs ) => lhs / rhs ) );
            myProviders.Add( new GenericJoinProvider( ProviderNames.BookValue, typeof( Equity ).Name, typeof( SharesOutstanding ).Name, 
                ( lhs, rhs ) => lhs / rhs ) );
            myProviders.Add( new GenericJoinProvider( ProviderNames.DividendPayoutRatio, typeof( Dividend ).Name, typeof( NetIncome ).Name,
                ( lhs, rhs ) => lhs / rhs * 100 ) { PreserveCurrency = false } );
            myProviders.Add( new GenericJoinProvider( ProviderNames.ReturnOnEquity, typeof( NetIncome ).Name, typeof( Equity ).Name,
                ( lhs, rhs ) => lhs / rhs * 100 ) { PreserveCurrency = false } );
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

        public IEnumerable<T> GetSeries<T>( string name )
        {
            var provider = myProviders.SingleOrDefault( p => p.Name == name );
            Contract.RequiresNotNull( provider, "No provider found with name: " + name );

            var series = ( Series )provider.ProvideValue( this );
            return series.Values.Cast<T>();
        }

        public void EnsureCurrencyConsistency( params IEnumerable<ICurrencyDatum>[] values )
        {
            var allCurrencies = values
                .SelectMany( v => v )
                .Select( v => v.Currency )
                .Distinct()
                .ToList();

            Contract.Requires( allCurrencies.Count == 1, "Currency inconsistencies found: {0}", string.Join( ",", allCurrencies ) );
        }
    }
}
