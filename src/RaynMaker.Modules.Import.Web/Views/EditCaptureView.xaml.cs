using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Modules.Import.Web.ViewModels;

namespace RaynMaker.Modules.Import.Web.Views
{
    [Export]
    public partial class EditCaptureView : UserControl
    {
        [ImportingConstructor]
        internal EditCaptureView( EditCaptureViewModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
