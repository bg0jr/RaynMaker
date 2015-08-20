using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;

namespace RaynMaker.Blade
{
    [Export]
    public partial class Shell : UserControl
    {
        [ImportingConstructor]
        internal Shell( ShellViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            Loaded += Shell_Loaded;
        }

        void Shell_Loaded( object sender, RoutedEventArgs e )
        {
            // workaround: only if we call this here the regions are updated on the interactionrequests put in this view
            RegionManager.UpdateRegions();
        }
    }
}
