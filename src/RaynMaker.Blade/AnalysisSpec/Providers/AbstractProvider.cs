using System.Collections.Generic;
using Plainion;
using RaynMaker.Blade.DataSheetSpec;
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
        
            PreserveCurrency = true;
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

        /// <summary>
        /// Indicates that result should take over currency of input.
        /// </summary>
        public bool PreserveCurrency { get; set; }
        
        public abstract object ProvideValue( IFigureProviderContext context );
       
        protected void EnsureCurrencyConsistancy( IDatumSeries lhs, IDatumSeries rhs )
        {
            Contract.Requires( lhs.Currency == null || rhs.Currency == null || lhs.Currency == rhs.Currency,
                "Currency inconsistencies detected: {0}.Currency={1} vs {2}.Currency={3}",
                lhs.Name, lhs.Currency, rhs.Name, rhs.Currency );
        }
    }
}
