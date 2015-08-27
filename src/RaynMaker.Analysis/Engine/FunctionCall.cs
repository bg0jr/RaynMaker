using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plainion;

namespace RaynMaker.Blade.Engine
{
    class FunctionCall
    {
        private ParameterInfo[] myParameters;
        private List<object> myArguments;

        public FunctionCall( MethodInfo function )
        {
            Contract.RequiresNotNull( function, "function" );

            Function = function;
            myParameters = Function.GetParameters();

            myArguments = new List<object>();
        }

        public MethodInfo Function { get; private set; }

        public IReadOnlyList<object> Arguments { get { return myArguments; } }

        public void AddArgument( object arg )
        {
            Contract.Invariant( myArguments.Count < myParameters.Length,
                "Function parameter count mismatch. Expected {0} but got {1}", myParameters.Length, Arguments.Count );

            if( arg == null )
            {
                myArguments.Add( null );
                return;
            }

            var parameter = myParameters[ myArguments.Count ];
            if( parameter.ParameterType.IsAssignableFrom( arg.GetType() ) )
            {
                myArguments.Add( arg );
            }
            else
            {
                var convertedArg = Convert.ChangeType( arg, parameter.ParameterType );
                myArguments.Add( convertedArg );
            }
        }

        public object Invoke()
        {
            Contract.Invariant( myArguments.Count == myParameters.Length,
                "Function parameter count mismatch. Expected {0} but got {1}", myParameters.Length, Arguments.Count );

            return Function.Invoke( null, Arguments.ToArray() );
        }
    }
}
