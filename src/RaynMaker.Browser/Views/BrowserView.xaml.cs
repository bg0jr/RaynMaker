using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Browser.ViewModels;

namespace RaynMaker.Browser.Views
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
