using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Analyzer.Services;

namespace RaynMaker.Analyzer.Views
{
    [Export]
    public partial class LogView : UserControl
    {
        [ImportingConstructor]
        internal LogView( LoggingSink sink )
        {
            InitializeComponent();

            DataContext = sink;
        }
    }
}
