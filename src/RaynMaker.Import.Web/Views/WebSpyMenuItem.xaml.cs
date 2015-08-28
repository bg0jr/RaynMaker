using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace RaynMaker.Import.Web
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
