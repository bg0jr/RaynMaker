using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Blade.ViewModels;

namespace RaynMaker.Blade.Views
{
    [Export]
    public partial class AnalysisTemplateEditView : UserControl
    {
        [ImportingConstructor]
        internal AnalysisTemplateEditView( AnalysisTemplateEditViewModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
