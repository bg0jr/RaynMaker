using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Data.ViewModels;

namespace RaynMaker.Data.Views
{
    [Export]
    public partial class TickerView : UserControl
    {
        [ImportingConstructor]
        internal TickerView( TickerViewModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
