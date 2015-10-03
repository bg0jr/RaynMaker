using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Notes;
using RaynMaker.Notes.Views;

namespace RaynMaker.Import.Web
{
    [ModuleExport( typeof( NotesModule ) )]
    public class NotesModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            RegionManager.RegisterViewWithRegion( RaynMaker.Infrastructure.RegionNames.Tools, typeof( NotesMenuItem ) );

            RegionManager.RegisterViewWithRegion( RaynMaker.Infrastructure.RegionNames.AssetContentPages, typeof( NotesContentPage ) );
            
            RegionManager.RegisterViewWithRegion( RegionNames.NotesView, typeof( NotesView ) );
        }
    }
}
