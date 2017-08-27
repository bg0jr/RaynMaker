using System.ComponentModel.Composition;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using RaynMaker.Modules.Import.Web.Views;

namespace RaynMaker.Modules.Import.Web
{
    [ModuleExport( typeof( ImportWebModule ) )]
    public class ImportWebModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            RegionManager.RegisterViewWithRegion( RaynMaker.Infrastructure.RegionNames.Tools, typeof( WebSpyMenuItem ) );

            RegionManager.RegisterViewWithRegion( RegionNames.WebSpyView, typeof( WebSpyView ) );
            RegionManager.RegisterViewWithRegion( RegionNames.EditCaptureView, typeof( EditCaptureView ) );
        }
    }
}
