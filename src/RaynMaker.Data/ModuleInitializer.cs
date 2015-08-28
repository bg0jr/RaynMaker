using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Data.Views;

namespace RaynMaker.Data
{
    [ModuleExport( typeof( ModuleInitializer ) )]
    public class ModuleInitializer : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            RegionManager.RegisterViewWithRegion( RaynMaker.Infrastructure.RegionNames.Tools, typeof( CurrenciesMenuItem ) );

            RegionManager.RegisterViewWithRegion( RaynMaker.Infrastructure.RegionNames.AssetContentPages, typeof( FactsContentPage ) );

            RegionManager.RegisterViewWithRegion( RegionNames.CurrenciesView, typeof( CurrenciesView ) );
            RegionManager.RegisterViewWithRegion( RegionNames.DataSheetEditView, typeof( FactsContentPage ) );
        }
    }
}
