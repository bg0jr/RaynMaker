using System;
using System.Collections.Generic;
using System.Linq;
using Plainion;

namespace RaynMaker.Blade.Engine
{
    class ExpressionEvaluator
    {
        private IEnumerable<IFigureProvider> myProviders;
        private IFigureProviderContext myContext;

        // TODO: how do we pass functions here? we want to have simple functions in UT
        public ExpressionEvaluator( IEnumerable<IFigureProvider> providers, IFigureProviderContext context )
        {
            myProviders = providers;
            myContext = context;
        }

        public object Evaluate( string expr )
        {
            return Compile( expr )();
        }

        private Func<object> Compile( string expr )
        {
            if(string.IsNullOrWhiteSpace(expr) )
            {
                return () => null;
            }

            return EvaluateWord( expr );
        }

        private Func<object> EvaluateWord( string word )
        {
            double result;
            if( double.TryParse( word, out result ) )
            {
                return () => result;
            }
            else
            {
                return () => GetProviderValue( word );
            }
        }

        private object GetProviderValue( string expr )
        {
            var tokens = expr.Split( '.' );
            var providerName = tokens.Length > 1 ? tokens[ 0 ] : expr;

            var provider = myProviders.SingleOrDefault( p => p.Name == providerName );
            Contract.Requires( provider != null, "{0} does not represent a IFigureProvider", providerName );

            var value = provider.ProvideValue( myContext );

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
    }
}
