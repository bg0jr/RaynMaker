using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Blade.Views;

namespace RaynMaker.Blade
{
    [ModuleExport( typeof( BladeModule ) )]
    public class BladeModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            RegionManager.RegisterViewWithRegion( RaynMaker.Infrastructure.RegionNames.Tools, typeof( AnalysisTemplateMenuItem ) );
            RegionManager.RegisterViewWithRegion( RaynMaker.Infrastructure.RegionNames.Tools, typeof( BladeMenuItem ) );

            RegionManager.RegisterViewWithRegion( RegionNames.Shell, typeof( Shell ) );

            RegionManager.RegisterViewWithRegion( RegionNames.CurrenciesView, typeof( CurrenciesView ) );
            RegionManager.RegisterViewWithRegion( RegionNames.AnalysisTemplateEditView, typeof( AnalysisTemplateEditView ) );
            RegionManager.RegisterViewWithRegion( RegionNames.DataSheetEditView, typeof( DataSheetEditView ) );
        }
    }
}
