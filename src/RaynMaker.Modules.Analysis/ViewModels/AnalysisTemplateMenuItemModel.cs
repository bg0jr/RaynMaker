using System.ComponentModel.Composition;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Mvvm;

namespace RaynMaker.Modules.Analysis.ViewModels
{
    [Export]
    public class AnalysisTemplateMenuItemModel : ToolMenuItemModelBase
    {
        [ImportingConstructor]
        public AnalysisTemplateMenuItemModel( IProjectHost projectHost )
            : base( projectHost, "Analysis template" )
        {
        }
    }
}
