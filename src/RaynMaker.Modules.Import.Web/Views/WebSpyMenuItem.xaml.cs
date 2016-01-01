using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Modules.Import.Web.ViewModels;

namespace RaynMaker.Modules.Import.Web.Views
{
    [Export]
    public partial class WebSpyMenuItem : MenuItem
    {
        [ImportingConstructor]
        public WebSpyMenuItem( WebSpyMenuItemModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
