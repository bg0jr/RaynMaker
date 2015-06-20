using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Analyzer.ViewModels;

namespace RaynMaker.Analyzer.Views
{
    [Export( CompositionNames.AssetDetailsView )]
    public partial class AssetDetailsView : UserControl
    {
        [ImportingConstructor]
        internal AssetDetailsView( AssetDetailsViewModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
