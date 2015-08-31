using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Import.Web.ViewModels;

namespace RaynMaker.Import.Web.Views
{
    [Export]
    public partial class InputMacroValueView : UserControl
    {
        [ImportingConstructor]
        internal InputMacroValueView(InputMacroValueViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
