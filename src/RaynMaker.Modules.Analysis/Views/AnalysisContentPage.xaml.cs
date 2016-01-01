using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Modules.Analysis.ViewModels;

namespace RaynMaker.Modules.Analysis.Views
{
    [Export]
    [ViewSortHint( "300" )]
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
