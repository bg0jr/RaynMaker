using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using RaynMaker.Analysis.ViewModels;

namespace RaynMaker.Analysis.Views
{
    [Export]
    [ViewSortHint( "200" )]
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
