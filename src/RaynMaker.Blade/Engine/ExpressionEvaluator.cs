using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Contract.Requires( expr.IndexOf( '.' ) == -1, "Property access not supported: ", expr );

            return EvaluateExpression( expr.RemoveAll( char.IsWhiteSpace ) );
        }

        // Average(Last(ReturnOnEquity,5))
        private object EvaluateExpression( string expr )
        {
            var openParenthesis = expr.IndexOf( '(' );
            if( openParenthesis > -1 )
            {
                var closeParenthesis = expr.LastIndexOf( ')' );
                Contract.ReferenceEquals( closeParenthesis > -1, "Expression error: missing closing parathesis" );

                var functionName = expr.Substring( 0, openParenthesis );
                var args = expr.Substring( openParenthesis + 1, closeParenthesis - openParenthesis )
                    .Split( new[] { ',' }, StringSplitOptions.None );

                return ExecuteFunction( functionName, args );
            }

            return GetProviderValue( expr );
        }

        private object ExecuteFunction( string functionName, string[] args )
        {
            var values = args
                .Select( arg => EvaluateExpression( arg ) )
                .ToArray();

            var functions = typeof( Functions ).GetMethods()
                .Where( m => m.Name == functionName )
                .Where( m => m.GetParameters().Length == values.Length )
                .ToList();

            Contract.Requires( functions.Count == 1, "None or multiple functions found with name '{0}' and {1} parameters", functionName, functions.Count );

            return functions.Single().Invoke( null, values );
        }

        private object GetProviderValue( string providerName )
        {
            var provider = myProviders.SingleOrDefault( p => p.Name == providerName );
            Contract.Requires( provider != null, "{0} does not represent a IFigureProvider", providerName );

            return provider.ProvideValue( myContext );
        }

    }
}
