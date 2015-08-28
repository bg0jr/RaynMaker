using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Analysis.ViewModels;

namespace RaynMaker.Analysis.Views
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
