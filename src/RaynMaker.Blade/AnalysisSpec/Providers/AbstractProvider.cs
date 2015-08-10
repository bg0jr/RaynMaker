using Plainion;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.AnalysisSpec.Providers
{
    public abstract class AbstractProvider : IFigureProvider
    {
        protected AbstractProvider( string name )
        {
            Contract.RequiresNotNullNotEmpty( name, "name" );

            Name = name;
        }

        public string Name { get; private set; }

        /// <summary>
        /// Provides additional information why the provider failed to provide the requested value (returned null).
        /// </summary>
        public string FailureReason { get; protected set; }

        public abstract object ProvideValue( IFigureProviderContext context );
    }
}
