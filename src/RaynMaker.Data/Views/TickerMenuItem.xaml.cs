using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Data.ViewModels;

namespace RaynMaker.Data.Views
{
    [Export]
    public partial class TickerMenuItem : MenuItem
    {
        [ImportingConstructor]
        public TickerMenuItem( TickerMenuItemModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
