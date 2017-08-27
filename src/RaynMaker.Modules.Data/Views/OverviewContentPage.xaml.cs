using System.ComponentModel.Composition;
using System.Windows.Controls;
using Prism.Regions;
using RaynMaker.Data.ViewModels;

namespace RaynMaker.Data.Views
{
    [Export]
    [ViewSortHint( "100" )]
    public partial class OverviewContentPage : UserControl
    {
        [ImportingConstructor]
        internal OverviewContentPage( OverviewContentPageModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
