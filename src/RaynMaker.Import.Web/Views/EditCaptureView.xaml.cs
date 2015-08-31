using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Import.Web.ViewModels;

namespace RaynMaker.Import.Web.Views
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
