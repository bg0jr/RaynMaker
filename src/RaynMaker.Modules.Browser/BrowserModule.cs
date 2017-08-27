using System.ComponentModel.Composition;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using RaynMaker.Modules.Browser.Views;

namespace RaynMaker.Modules.Browser
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
