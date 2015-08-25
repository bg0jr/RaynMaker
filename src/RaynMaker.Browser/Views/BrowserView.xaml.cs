using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Analyzer;
using RaynMaker.Browser.ViewModels;

namespace RaynMaker.Browser.Views
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
