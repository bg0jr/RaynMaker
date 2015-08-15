using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Blade.Views;

namespace RaynMaker.Blade
{
    [ModuleExport( typeof( CoreModule ) )]
    public class CoreModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            RegionManager.RegisterViewWithRegion( RegionNames.CurrenciesView, typeof( CurrenciesView ) );
            RegionManager.RegisterViewWithRegion( RegionNames.AnalysisTemplateEditView, typeof( AnalysisTemplateEditView ) );
            RegionManager.RegisterViewWithRegion( RegionNames.DataSheetEditView, typeof( DataSheetEditView ) );
        }
    }
}
