using System.ComponentModel.Composition;
using RaynMaker.Infrastructure;
using RaynMaker.Infrastructure.Mvvm;

namespace RaynMaker.Data.ViewModels
{
    [Export]
    public class CurrenciesMenuItemModel : ToolMenuItemModelBase
    {
        [ImportingConstructor]
        public CurrenciesMenuItemModel( IProjectHost projectHost )
            : base( projectHost, "Currencies" )
        {
        }
    }
}
