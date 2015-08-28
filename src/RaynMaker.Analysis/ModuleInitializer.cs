using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Analysis.Views;

namespace RaynMaker.Analysis
{
    [ModuleExport( typeof( ModuleInitializer ) )]
    public class ModuleInitializer : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            RegionManager.RegisterViewWithRegion( RaynMaker.Infrastructure.RegionNames.Tools, typeof( AnalysisTemplateMenuItem ) );

            RegionManager.RegisterViewWithRegion( RaynMaker.Infrastructure.RegionNames.AssetContentPages, typeof( AnalysisContentPage ) );

            RegionManager.RegisterViewWithRegion( RegionNames.AnalysisTemplateEditView, typeof( AnalysisTemplateEditView ) );
        }
    }
}
