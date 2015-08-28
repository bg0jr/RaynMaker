using System.ComponentModel.Composition;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Data.Services
{
    class LutService : ILutService
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public LutService( IProjectHost projectHost )
        {
            myProjectHost = projectHost;
            myProjectHost.Changed += OnProjectChanged;

            OnProjectChanged();
        }

        private void OnProjectChanged()
        {
            if( CurrenciesLut == null )
            {
                CurrenciesLut = new CurrenciesLut( myProjectHost );
            }

            CurrenciesLut.Reload();
        }

        public ICurrenciesLut CurrenciesLut { get; private set; }
    }
}
