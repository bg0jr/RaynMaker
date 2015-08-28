using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Data.ViewModels;

namespace RaynMaker.Data.Views
{
    [Export]
    public partial class CurrenciesMenuItem : MenuItem
    {
        [ImportingConstructor]
        public CurrenciesMenuItem( CurrenciesMenuItemModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
