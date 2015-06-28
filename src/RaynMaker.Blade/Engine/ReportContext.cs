using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public class ReportContext
    {
        private List<IFigureProvider> myProviders;

        public ReportContext( Asset asset, FlowDocument document )
        {
            Asset = asset;
            Document = document;

            myProviders = GetType().Assembly.GetTypes()
                .Where( t => t.GetInterfaces().Any( iface => iface == typeof( IFigureProvider ) ) )
                .Where( t => t != typeof( GenericSeriesProvider ) )
                .Select( t => Activator.CreateInstance( t ) )
                .OfType<IFigureProvider>()
                .ToList();
            myProviders.Add( new GenericSeriesProvider( typeof( SharesOutstanding ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( NetIncome ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( Equity ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( Eps ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( Dividend ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( Assets ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( Liabilities ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( Dept ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( Revenue ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( EBIT ) ) );
            myProviders.Add( new GenericSeriesProvider( typeof( InterestExpense ) ) );
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
            var provider = myProviders.SingleOrDefault( p => p.Name == providerName );
            var value = provider.ProvideValue( Asset );

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
    }
}
