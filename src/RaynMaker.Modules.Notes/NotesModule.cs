﻿using System.ComponentModel.Composition;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using RaynMaker.Modules.Notes;
using RaynMaker.Modules.Notes.Views;

namespace RaynMaker.Modules.Notes
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
