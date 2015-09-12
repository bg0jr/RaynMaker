using System.Windows;
using RaynMaker.Import.Web.ViewModels;

namespace RaynMaker.Import.Web.Views
{
    public partial class ImportPreview : Window
    {
        private ImportPreviewModel myViewModel;

        internal ImportPreview( ImportPreviewModel viewModel )
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
