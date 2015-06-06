using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Browser.ViewModels;

namespace RaynMaker.Browser.Views
{
    [Export]
    public partial class NewAssetView : UserControl
    {
        [ImportingConstructor]
        NewAssetView( NewAssetViewModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
