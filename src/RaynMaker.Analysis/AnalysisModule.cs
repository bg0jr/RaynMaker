using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace RaynMaker.Analysis
{
    [ModuleExport( typeof( AnalysisModule ) )]
    public class AnalysisModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
        }
    }
}
