using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace RaynMaker.Blade
{
    [Export( typeof( BladeMenuItem ) )]
    public partial class BladeMenuItem : MenuItem
    {
        [ImportingConstructor]
        public BladeMenuItem( BladeMenuItemModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
