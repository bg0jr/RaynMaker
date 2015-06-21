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

        public ReportContext( Asset asset, TextWriter writer )
        {
            Asset = asset;
            Out = writer;

            myProviders = GetType().Assembly.GetTypes()
                .Where( t => t.GetInterfaces().Any( iface => iface == typeof( IFigureProvider ) ) )
                .Select( t => Activator.CreateInstance( t ) )
                .OfType<IFigureProvider>()
                .ToList();
        }

        public Asset Asset { get; private set; }

        public TextWriter Out { get; private set; }

        public string Evaluate( string text )
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
                sb.Append( EvaluateExpression( expr ) );

                start = pos + 1;
            }

            return sb.ToString();
        }

        private string EvaluateExpression( string expr )
        {
            var tokens = expr.Split( '.' );
            var providerName = tokens.Length > 1 ? tokens[ 0 ] : expr;
            var provider = myProviders.SingleOrDefault( p => p.Name == providerName );
            var value = provider.ProvideValue( Asset );

            if( tokens.Length == 1 )
            {
                return value.ToString();
            }

            foreach( var token in tokens.Skip( 1 ) )
            {
                value = GetValue( value, token );
            }

            return value.ToString();
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
    }
}
