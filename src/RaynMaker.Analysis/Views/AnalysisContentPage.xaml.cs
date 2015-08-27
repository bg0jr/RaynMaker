using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Blade.ViewModels;

namespace RaynMaker.Blade.Views
{
    [Export]
    public partial class AnalysisContentPage : UserControl
    {
        [ImportingConstructor]
        internal AnalysisContentPage( AnalysisContentPageModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
