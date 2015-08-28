using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace RaynMaker.Analysis
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
