using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace RaynMaker.Blade
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
