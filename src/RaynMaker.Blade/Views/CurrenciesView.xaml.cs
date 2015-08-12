using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Blade.ViewModels;

namespace RaynMaker.Blade.Views
{
    [Export]
    public partial class CurrenciesView : UserControl
    {
        [ImportingConstructor]
        internal CurrenciesView(CurrenciesViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
