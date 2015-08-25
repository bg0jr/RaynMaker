using Microsoft.Practices.Prism.Regions;
using RaynMaker.Entities;

namespace RaynMaker.Analyzer.Services
{
    class AssetNavigationParameters
    {
        public AssetNavigationParameters()
            : this( new NavigationParameters() )
        {
        }

        public AssetNavigationParameters( NavigationParameters parameters )
        {
            Parameters = parameters;
        }

        public NavigationParameters Parameters { get; private set; }

        public Stock Stock
        {
            get { return ( Stock )Parameters[ "Stock" ]; }
            set { Parameters.Add( "Stock", value ); }
        }
    }
}
