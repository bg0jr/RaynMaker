using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Browser.Views;

namespace RaynMaker.Browser
{
    [ModuleExport( typeof( ModuleInitializer ) )]
    public class ModuleInitializer : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            RegionManager.RegisterViewWithRegion( RegionNames.NewAssetView, typeof( NewAssetView ) );
        }
    }
}
