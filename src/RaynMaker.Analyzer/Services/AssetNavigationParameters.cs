using Microsoft.Practices.Prism.Regions;

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

        public long AssetId
        {
            get { return ( long )Parameters[ "AssetId" ]; }
            set { Parameters.Add( "AssetId", value ); }
        }
    }
}
