using System.Collections.Generic;
using Plainion;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public abstract class AbstractProvider : IFigureProvider
    {
        private List<string> myFailureReasons;

        protected AbstractProvider( string name )
        {
            Contract.RequiresNotNullNotEmpty( name, "name" );

            Name = name;
            myFailureReasons = new List<string>();
        }

        public string Name { get; private set; }

        /// <summary>
        /// Provides additional information why the provider failed to provide the requested value (returned null).
        /// </summary>
        public IReadOnlyCollection<string> FailureReasons { get { return myFailureReasons; } }

        protected void AddFailureReason( string fmt, params object[] args )
        {
            myFailureReasons.Add( string.Format( fmt, args ) );
        }

        public abstract object ProvideValue( IFigureProviderContext context );
    }
}
