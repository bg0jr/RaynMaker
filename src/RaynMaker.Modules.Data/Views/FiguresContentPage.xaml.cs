using System.ComponentModel.Composition;
using System.Windows.Controls;
using Prism.Regions;
using RaynMaker.Data.ViewModels;

namespace RaynMaker.Data.Views
{
    [Export]
    [ViewSortHint( "200" )]
    public partial class FiguresContentPage : UserControl
    {
        [ImportingConstructor]
        internal FiguresContentPage( FiguresContentPageModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
