using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Data.ViewModels;

namespace RaynMaker.Data.Views
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
