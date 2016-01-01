using System.ComponentModel.Composition;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Mvvm;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    public class TickerMenuItemModel : ToolMenuItemModelBase
    {
        [ImportingConstructor]
        public TickerMenuItemModel( IProjectHost projectHost )
            : base( projectHost, "Ticker" )
        {
        }
    }
}
