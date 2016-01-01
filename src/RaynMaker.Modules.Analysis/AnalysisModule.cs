using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Modules.Analysis.Views;

namespace RaynMaker.Modules.Analysis
{
    [ModuleExport( typeof( AnalysisModule ) )]
    public class AnalysisModule : IModule
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
