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
        private IExpressionEvaluationContext myContext;
        private IEnumerable<MethodInfo> myFunctions;
        private ParseData myData;

        private class TokenCollection
        {
            private string[] myValues;
            private static Regex SplitPattern = new Regex( @"([.\(\),])" );

            public TokenCollection( string expr )
            {
                myValues = SplitPattern.Split( expr )
                    .Select( t => t.Trim() )
                    .Where( t => !string.IsNullOrEmpty( t ) )
                    .ToArray();

                Index = -1;
            }

            public int Index;

            public string this[ int index ] { get { return myValues[ index ]; } }

            public int Count { get { return myValues.Length; } }

            public string Current { get { return myValues[ Index ]; } }

            public string LookAhead( int count )
            {
                var pos = Index + count;
                return pos < myValues.Length ? myValues[ pos ] : null;
            }
        }

        private class ParseData
        {
            public TokenCollection Tokens { get; private set; }
            public object Result;
            public Stack<FunctionCall> Callstack { get; private set; }

            public ParseData( string expr )
            {
                Tokens = new TokenCollection( expr );
                Result = null;
                Callstack = new Stack<FunctionCall>();
            }
        }

        private enum ParseState
        {
            Start,
            HaveValue,
            CallMember,
            CompleteDecimal,
        }

        public ExpressionEvaluator( IExpressionEvaluationContext context )
            : this( context, null )
        {
        }

        public ExpressionEvaluator( IExpressionEvaluationContext context, Type functionsDefinition )
        {
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

            try
            {
                myData = new ParseData( expr );
                var state = ParseState.Start;

                for( myData.Tokens.Index = 0; myData.Tokens.Index < myData.Tokens.Count; ++myData.Tokens.Index )
                {
                    var token = myData.Tokens[ myData.Tokens.Index ];

                    if( state == ParseState.Start )
                    {
                        if( myData.Tokens.LookAhead( 1 ) == "(" )
                        {
                            PushFunction();
                            state = ParseState.Start;
                        }
                        else if( token == ")" )
                        {
                            PopFunction();
                            state = ParseState.HaveValue;
                        }
                        else
                        {
                            EvaluateWord();
                            state = ParseState.HaveValue;
                        }
                    }
                    else if( state == ParseState.HaveValue )
                    {
                        if( token == "," )
                        {
                            ConsumeFunctionParameter();
                            state = ParseState.Start;
                        }
                        else if( token == ")" )
                        {
                            ConsumeFunctionParameter();
                            PopFunction();
                            state = ParseState.HaveValue;
                        }
                        else if( token == "." )
                        {
                            if( IsIncompleteDecimal() )
                            {
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
                        CompleteDecimal();
                        state = ParseState.HaveValue;
                    }
                    else if( state == ParseState.CallMember )
                    {
                        CallMember();
                        state = ParseState.HaveValue;
                    }
                    else
                    {
                        throw new NotImplementedException( "I missed to implement a state in the parser :(" );
                    }
                }

                return myData.Result;
            }
            catch( Exception ex )
            {
                throw new InvalidOperationException( string.Format( "Failed to parse expression '{0}' at token {1}", expr, myData.Tokens.Index ), ex );
            }
            finally
            {
                myData = null;
            }
        }

        private void CompleteDecimal()
        {
            myData.Result = double.Parse( myData.Result + "." + myData.Tokens.Current, CultureInfo.InvariantCulture );
        }

        // check if dot was splitting decimal - complete decimal
        private bool IsIncompleteDecimal()
        {
            var lookAHead = myData.Tokens.LookAhead( 1 );
            double ignore;
            return lookAHead != null && double.TryParse( lookAHead, out ignore );
        }

        private void ConsumeFunctionParameter()
        {
            Contract.Requires( myData.Callstack.Peek() != null, "Function context expected" );

            myData.Callstack.Peek().AddArgument( myData.Result );
            myData.Result = null;
        }

        private void PopFunction()
        {
            Contract.Requires( myData.Callstack.Peek() != null, "Function context expected" );
            Contract.Invariant( myData.Result == null, "No result expected" );

            myData.Result = myData.Callstack.Pop().Invoke();
        }

        private void PushFunction()
        {
            var token = myData.Tokens.Current;

            var function = myFunctions.SingleOrDefault( f => f.Name == token );
            Contract.Requires( function != null, "No function of name '{0}' found", token );

            myData.Callstack.Push( new FunctionCall( function ) );
            myData.Tokens.Index++;
        }

        private void CallMember()
        {
            if( myData.Tokens.LookAhead( 1 ) == "(" )
            {
                CallMethod();
            }
            else
            {
                CallProperty();
            }
        }

        private void CallMethod()
        {
            Contract.Requires( myData.Tokens.Index + 2 < myData.Tokens.Count, "')' misssing at " + ( myData.Tokens.Index + 1 ) );
            Contract.Requires( myData.Tokens[ myData.Tokens.Index + 2 ] == ")", "Parameters not supported at " + ( myData.Tokens.Index + 2 ) );

            Contract.Invariant( myData.Result != null, "Cannot call method '{0}' on null", myData.Tokens.Current );

            var method = myData.Result.GetType().GetMethod( myData.Tokens.Current );

            Contract.Requires( method != null, "'{0}' does not have a method named '{1}'", myData.Result.GetType(), myData.Tokens.Current );

            myData.Result = method.Invoke( myData.Result, null );

            myData.Tokens.Index += 2;
        }

        private void CallProperty()
        {
            Contract.Invariant( myData.Result != null, "Cannot call property '{0}' on null", myData.Tokens.Current );

            var property = myData.Result.GetType().GetProperty( myData.Tokens.Current );

            Contract.Requires( property != null, "'{0}' does not have a property named '{1}'", myData.Result.GetType(), myData.Tokens.Current );

            myData.Result = property.GetValue( myData.Result );
        }

        private void EvaluateWord()
        {
            var word = myData.Tokens.Current;

            double result;
            if( double.TryParse( word, out result ) )
            {
                myData.Result = result;
            }
            else
            {
                myData.Result = myContext.ProvideValue( word );
            }
        }
    }
}
