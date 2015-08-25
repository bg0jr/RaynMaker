using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer.Services
{
    [Export( typeof( AssetNavigationService ) )]
    class AssetNavigationService
    {
        private IRegionManager myRegionManager;
        private IRegion myRegion;

        [ImportingConstructor]
        public AssetNavigationService( IRegionManager regionManager )
        {
            myRegionManager = regionManager;

            RegisterRegionOnDemand();
        }

        // if this service is fetched before the Xaml is loaded which defines the region then the region will not exist.
        private void RegisterRegionOnDemand()
        {
            if( myRegion != null )
            {
                return;
            }

            if( !myRegionManager.Regions.ContainsRegionWithName( RegionNames.Content ) )
            {
                return;
            }

            myRegion = myRegionManager.Regions[ RegionNames.Content ];
            myRegion.NavigationService.NavigationFailed += OnNavigationFailed;
        }

        private void OnNavigationFailed( object sender, RegionNavigationFailedEventArgs e )
        {
            throw new InvalidOperationException( "Navigation failed", e.Error );
        }

        public void NavigateToBrowser()
        {
            RegisterRegionOnDemand();

            var args = new AssetNavigationParameters();

            myRegionManager.RequestNavigate( RegionNames.Content, new Uri( CompositionNames.BrowserView, UriKind.Relative ), args.Parameters );
        }

        public void NavigateToAsset( long assetId )
        {
            RegisterRegionOnDemand();

            var args = new AssetNavigationParameters();
            args.AssetId = assetId;

            myRegionManager.RequestNavigate( RegionNames.Content, new Uri( CompositionNames.AssetDetailsView, UriKind.Relative ), args.Parameters );
        }
    }
}
