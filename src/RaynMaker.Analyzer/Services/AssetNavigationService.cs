using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Analyzer.Services
{
    [Export]
    [Export( typeof( IAssetNavigation ) )]
    class AssetNavigationService : IAssetNavigation
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
            throw e.Error;
        }

        public void NavigateToBrowser()
        {
            RegisterRegionOnDemand();

            var args = new AssetNavigationParameters();

            myRegionManager.RequestNavigate( RegionNames.Content, new Uri( CompositionNames.BrowserView, UriKind.Relative ), args.Parameters );
        }

        public void NavigateToAsset( Stock stock )
        {
            RegisterRegionOnDemand();

            var args = new AssetNavigationParameters();
            args.Stock = stock;

            myRegionManager.RequestNavigate( RegionNames.Content, new Uri( InternalCompositionNames.AssetMasterPage, UriKind.Relative ), args.Parameters );
        }

        public void ClosePage( object page )
        {
            var view = myRegion.Views.Single( v => IsPage( v, page ) );
            myRegion.Remove( view );
        }

        private bool IsPage( object view, object page )
        {
            if( view == page )
            {
                return true;
            }

            var frameworkElement = view as FrameworkElement;
            if( frameworkElement == null )
            {
                return false;
            }

            return frameworkElement.DataContext == page;
        }
    }
}
