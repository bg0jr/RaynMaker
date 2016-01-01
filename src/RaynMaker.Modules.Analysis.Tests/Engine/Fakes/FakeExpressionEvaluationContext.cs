using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaynMaker.Modules.Analysis.Engine;

namespace RaynMaker.Modules.Analysis.UnitTests.Engine.Fakes
{
    class FakeExpressionEvaluationContext : IExpressionEvaluationContext
    {
        private IEnumerable<IFigureProvider> myProviders;
        private FakeFigureProviderContext myContext;

        public FakeExpressionEvaluationContext( IEnumerable<IFigureProvider> providers )
        {
            myProviders = providers;
            myContext = new FakeFigureProviderContext();
        }

        public object ProvideValue( string providerName )
        {
            var provider = myProviders.SingleOrDefault( p => p.Name == providerName );
            return provider.ProvideValue( myContext );
        }
    }
}
