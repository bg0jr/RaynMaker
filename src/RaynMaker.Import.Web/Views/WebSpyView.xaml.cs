using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using RaynMaker.Import.Web.ViewModels;

namespace RaynMaker.Import.Web.Views
{
    [Export]
    public partial class WebSpyView : UserControl
    {
        private WebSpyViewModel myViewModel;

        [ImportingConstructor]
        internal WebSpyView( WebSpyViewModel viewModel )
        {
            myViewModel = viewModel;

            InitializeComponent();

            DataContext = viewModel;

            Loaded += WebSpyView_Loaded;
        }

        private void WebSpyView_Loaded( object sender, RoutedEventArgs e )
        {
            myViewModel.Browser = myBrowser;
        }
    }
}
