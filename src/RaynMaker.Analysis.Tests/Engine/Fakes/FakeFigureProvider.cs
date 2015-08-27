using System;
using RaynMaker.Blade.Engine;

namespace RaynMaker.Blade.Tests.Engine.Fakes
{
    class FakeFigureProvider : IFigureProvider
    {
        private Func<IFigureProviderContext, object> myProvideValueDelegate;

        public FakeFigureProvider( string name, Func<IFigureProviderContext, object> ProvideValue )
        {
            Name = name;
            myProvideValueDelegate = ProvideValue;
        }

        public string Name { get; private set; }

        public object ProvideValue( IFigureProviderContext context )
        {
            return myProvideValueDelegate( context );
        }
    }
}
