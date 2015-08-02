using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Plainion;

namespace RaynMaker.Blade.Engine
{
    class ExpressionEvaluator
    {
        private IEnumerable<IFigureProvider> myProviders;
        private IFigureProviderContext myContext;
        private IEnumerable<MethodInfo> myFunctions;

        private enum ParseState
        {
            Start,
            HaveValue,
            CallMember,
            CompleteDecimal,
        }

        private class FunctionCall
        {
            public FunctionCall( MethodInfo function )
            {
                Function = function;
                Arguments = new List<object>();
            }

            public MethodInfo Function { get; private set; }

            public IList<object> Arguments { get; private set; }

            public object Invoke()
            {
                return Function.Invoke( null, Arguments.ToArray() );
            }
        }

        public ExpressionEvaluator( IEnumerable<IFigureProvider> providers, IFigureProviderContext context )
            : this( providers, context, null )
        {
        }

        public ExpressionEvaluator( IEnumerable<IFigureProvider> providers, IFigureProviderContext context, Type functionsDefinition )
        {
            myProviders = providers;
            myContext = context;

            if( functionsDefinition != null )
            {
                myFunctions = functionsDefinition.GetMethods( BindingFlags.Public | BindingFlags.Static );
            }
            else
            {
                myFunctions = Enumerable.Empty<MethodInfo>();
            }
        }

        public object Evaluate( string expr )
        {
            if( string.IsNullOrWhiteSpace( expr ) )
            {
                return null;
            }

            var tokens = Regex.Split( expr, @"([.\(\),])" )
                .Select( t => t.Trim() )
                .Where( t => !string.IsNullOrEmpty( t ) )
                .ToList();

            object result = null;
            var state = ParseState.Start;
            var callstack = new Stack<FunctionCall>();
            for( int i = 0; i < tokens.Count; ++i )
            {
                var token = tokens[ i ];

                if( state == ParseState.Start )
                {
                    if( i + 1 < tokens.Count && tokens[ i + 1 ] == "(" )
                    {
                        var function = myFunctions.SingleOrDefault( f => f.Name == token );
                        Contract.Requires( function != null, "No function of name '{0}' found", token );

                        callstack.Push( new FunctionCall( function ) );
                        i++;

                        state = ParseState.Start;
                    }
                    else if( token == ")" )
                    {
                        Contract.Requires( callstack.Peek() != null, "Expected to be in function call because ',' was found" );
                        Contract.Requires( result == null, "No result expected" );

                        result = callstack.Pop().Invoke();

                        state = ParseState.HaveValue;
                    }
                    else
                    {
                        result = EvaluateWord( token );
                        state = ParseState.HaveValue;
                    }
                }
                else if( state == ParseState.HaveValue )
                {
                    if( token == "," )
                    {
                        Contract.Requires( callstack.Peek() != null, "Expected to be in function call because ',' was found" );

                        callstack.Peek().Arguments.Add( result );
                        result = null;

                        state = ParseState.Start;
                    }
                    else if( token == ")" )
                    {
                        Contract.Requires( callstack.Peek() != null, "Expected to be in function call because ',' was found" );

                        var function = callstack.Pop();
                        function.Arguments.Add( result );
                        result = function.Invoke();

                        state = ParseState.HaveValue;
                    }
                    else if( token == "." )
                    {
                        double ignore;
                        if( i + 1 < tokens.Count && double.TryParse( tokens[ i + 1 ], out ignore ) )
                        {
                            // dot was splitting decimal - complete decimal
                            state = ParseState.CompleteDecimal;
                        }
                        else
                        {
                            state = ParseState.CallMember;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException( "Don't know how to handle token '" + token + "'" );
                    }
                }
                else if( state == ParseState.CompleteDecimal )
                {
                    result = double.Parse( result + "." + token, CultureInfo.InvariantCulture );
                    state = ParseState.HaveValue;
                }
                else if( state == ParseState.CallMember )
                {
                    if( i + 1 < tokens.Count && tokens[ i + 1 ] == "(" )
                    {
                        Contract.Requires( i + 2 < tokens.Count, "')' misssing at " + ( i + 1 ) );
                        Contract.Requires( tokens[ i + 2 ] == ")", "no parameters supported at " + ( i + 2 ) );

                        var method = result.GetType().GetMethod( token );

                        Contract.Requires( method != null, "'{0}' does not have a method named '{1}'", result.GetType(), token );

                        result = method.Invoke( token, null );

                        i += 2;
                    }
                    else
                    {
                        var property = result.GetType().GetProperty( token );

                        Contract.Requires( property != null, "'{0}' does not have a property named '{1}'", result.GetType(), token );

                        result = property.GetValue( result );
                    }

                    state = ParseState.HaveValue;
                }
                else
                {
                    throw new NotImplementedException( "I missed to implement a state in the parser :(" );
                }
            }

            return result;
        }

        private object EvaluateWord( string word )
        {
            double result;
            if( double.TryParse( word, out result ) )
            {
                return result;
            }
            else
            {
                var provider = myProviders.SingleOrDefault( p => p.Name == word );
                Contract.Requires( provider != null, "{0} does not represent a IFigureProvider", word );

                return provider.ProvideValue( myContext );
            }
        }
    }
}
