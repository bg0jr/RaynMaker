using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace RaynMaker.Data
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
