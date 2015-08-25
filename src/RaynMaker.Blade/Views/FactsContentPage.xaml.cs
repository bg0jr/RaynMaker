using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Blade.ViewModels;

namespace RaynMaker.Blade.Views
{
    [Export]
    public partial class FactsContentPage : UserControl
    {
        [ImportingConstructor]
        internal FactsContentPage( FactsContentPageModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
