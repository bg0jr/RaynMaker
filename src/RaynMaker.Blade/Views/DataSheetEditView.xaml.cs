using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Blade.ViewModels;

namespace RaynMaker.Blade.Views
{
    [Export]
    public partial class DataSheetEditView : UserControl
    {
        [ImportingConstructor]
        internal DataSheetEditView(DataSheetEditViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
