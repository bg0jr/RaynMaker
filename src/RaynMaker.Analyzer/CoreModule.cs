using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Analyzer.Views;

namespace RaynMaker.Analyzer
{
    [ModuleExport( typeof( CoreModule ) )]
    public class CoreModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            RegionManager.RegisterViewWithRegion( "Views.Log", typeof( LogView ) );
        }
    }
}
