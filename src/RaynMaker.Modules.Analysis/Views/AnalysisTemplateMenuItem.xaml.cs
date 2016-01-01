using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Modules.Analysis.ViewModels;

namespace RaynMaker.Modules.Analysis.Views
{
    [Export]
    public partial class AnalysisTemplateMenuItem : MenuItem
    {
        [ImportingConstructor]
        public AnalysisTemplateMenuItem( AnalysisTemplateMenuItemModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
