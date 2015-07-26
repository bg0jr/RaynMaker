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
            var tokens = expr.RemoveAll( char.IsWhiteSpace )
                .Split( new[] { ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries );

            return EvaluateExpression( tokens );
        }

        // Average(Last(ReturnOnEquity,5))
        private Func<object> EvaluateExpression( string[] tokens )
        {
            if( tokens.Length == 0 )
            {
                return () => null;
            }

            if( tokens.Length == 1 )
            {
                return EvaluateWord( tokens[ 0 ] );
            }

            Contract.Requires( tokens.Length >= 4, "Invalid function call: {0}", string.Join( "", tokens ) );
            Contract.Requires( tokens[ 1 ] == "(", "'(' expected: {0}", string.Join( "", tokens ) );
            Contract.Requires( tokens[ tokens.Length - 1 ] == ")", "Missing ')': {0}", string.Join( "", tokens ) );

            var functionName = tokens[ 0 ];
            var args = EvaluateArguments( tokens.Skip( 2 ).Take( tokens.Length - 3 ).ToArray() );

            return ExecuteFunction( functionName, args );
        }

        private object[] EvaluateArguments( string[] p )
        {
            throw new NotImplementedException();
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

        private Func<object> ExecuteFunction( string functionName, object[] args )
        {
            var functions = typeof( Functions ).GetMethods()
                .Where( m => m.Name == functionName )
                .Where( m => m.GetParameters().Length == args.Length )
                .ToList();

            Contract.Requires( functions.Count == 1, "None or multiple functions found with name '{0}' and {1} parameters", functionName, functions.Count );

            return () => functions.Single().Invoke( null, args );
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
