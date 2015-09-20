using System.ComponentModel.Composition;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Mvvm;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    public class WebSpyMenuItemModel : ToolMenuItemModelBase
    {
        [ImportingConstructor]
        public WebSpyMenuItemModel( IProjectHost projectHost )
            : base( projectHost, "Web Spy" )
        {
        }
    }
}
