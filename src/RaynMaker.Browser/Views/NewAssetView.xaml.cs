using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using RaynMaker.Browser.ViewModels;
using RaynMaker.Infrastructure.Mvvm;

namespace RaynMaker.Browser.Views
{
    [Export]
    public partial class NewAssetView : UserControl
    {
        [ImportingConstructor]
        NewAssetView( NewAssetViewModel viewModel )
        {
            InitializeComponent();

            DataContext = viewModel;

            Loaded += OnLoaded;
        }

        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            var validationAware = DataContext as IValidationAware;
            if( validationAware != null )
            {
                validationAware.ValidateProperties();
            }
        }
    }
}
