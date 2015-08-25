using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Browser.Views;

namespace RaynMaker.Browser
{
    [ModuleExport( typeof( BrowserModule ) )]
    public class BrowserModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            RegionManager.RegisterViewWithRegion( RegionNames.NewAssetView, typeof( NewAssetView ) );
        }
    }
}
