using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace RaynMaker.Browser
{
    [Export]
    public partial class BrowserView : UserControl
    {
        [ImportingConstructor]
        BrowserView( BrowserViewModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
