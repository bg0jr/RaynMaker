using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace RaynMaker.Import.Web
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
        }
    }
}
