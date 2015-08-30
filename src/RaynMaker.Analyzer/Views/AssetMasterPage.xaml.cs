using System.ComponentModel.Composition;
using System.Windows.Controls;
using RaynMaker.Analyzer.ViewModels;

namespace RaynMaker.Analyzer.Views
{
    [Export( InternalCompositionNames.AssetMasterPage )]
    public partial class AssetMasterPage : UserControl
    {
        [ImportingConstructor]
        internal AssetMasterPage( AssetMasterPageModel model )
        {
            InitializeComponent();

            DataContext = model;
        }
    }
}
