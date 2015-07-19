using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using Plainion;
using RaynMaker.Blade.AnalysisSpec.Functions;
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
                .Where( t => t != typeof( DatumSeries ) )
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
        }

        public Asset Asset { get; private set; }

        public FlowDocument Document { get; private set; }

        internal string Evaluate( string text )
        {
            var sb = new StringBuilder();

            int start = 0;
            while( -1 < start && start < text.Length )
            {
                var pos = text.IndexOf( "${", start );
                if( pos == -1 )
                {
                    sb.Append( text.Substring( start ) );
                    break;
                }

                sb.Append( text.Substring( start, pos - start ) );

                start = pos + 2;

                pos = text.IndexOf( "}", start );
                if( pos == -1 )
                {
                    sb.Append( text.Substring( start, pos - start ) );
                    break;
                }

                var expr = text.Substring( start, pos - start );
                sb.Append( FormatValue( EvaluateExpression( expr ) ) );

                start = pos + 1;
            }

            return sb.ToString();
        }

        private string FormatValue( object value )
        {
            if( value == null )
            {
                return "n.a.";
            }
            else if( value is double )
            {
                return ( ( double )value ).ToString( "0.00" );
            }
            else
            {
                return value.ToString();
            }
        }

        private object EvaluateExpression( string expr )
        {
            var tokens = expr.Split( '.' );
            var providerName = tokens.Length > 1 ? tokens[ 0 ] : expr;
            var provider = myProviders.Single( p => p.Name == providerName );
            var value = provider.ProvideValue( this );

            if( tokens.Length == 1 )
            {
                return value;
            }

            if( value == null )
            {
                return null;
            }

            foreach( var token in tokens.Skip( 1 ) )
            {
                value = GetValue( value, token );
            }

            return value;
        }

        private object GetValue( object value, string member )
        {
            if( member.EndsWith( ")" ) )
            {
                var method = value.GetType().GetMethod( member.Substring( 0, member.IndexOf( '(' ) ) );

                Contract.Requires( method != null, "'{0}' does not have a method named '{1}'", value.GetType(), member );

                return method.Invoke( value, null );
            }
            else
            {
                var property = value.GetType().GetProperty( member );

                Contract.Requires( property != null, "'{0}' does not have a property named '{1}'", value.GetType(), member );

                return property.GetValue( value );
            }
        }

        internal IFigureProvider GetProvider( string expr )
        {
            Contract.Requires( expr.StartsWith( "${" ) && expr.EndsWith( "}" ), "Not an expression: " + expr );

            var path = expr.Substring( 2, expr.Length - 3 );

            Contract.Requires( path.IndexOf( '.' ) == -1, "Nested providers not supported: ", expr );

            var provider = myProviders.SingleOrDefault( p => p.Name == path );

            Contract.Requires( provider != null, "{0} does not represent a IFigureProvider", expr );

            return provider;
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

        public IEnumerable<T> GetDatumSeries<T>()
        {
            var series = Asset.Data.OfType<Series>()
                .Where( s => s.Values.OfType<T>().Any() )
                .SingleOrDefault();

            if( series == null )
            {
                return Enumerable.Empty<T>();
            }

            return series.Values.Cast<T>();
        }

        public IEnumerable<T> GetCalculatedSeries<T>( string name )
        {
            var provider = myProviders.Single( p => p.Name == name );
            var series= ( Series )provider.ProvideValue( this );
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
