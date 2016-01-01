using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Analyzer;
using RaynMaker.Modules.Browser.ViewModels;

namespace RaynMaker.Modules.Browser.Views
{
    [Export( CompositionNames.BrowserView )]
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
