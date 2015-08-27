using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Blade.Views;

namespace RaynMaker.Blade
{
    [ModuleExport( typeof( DataModule ) )]
    public class DataModule : IModule
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
