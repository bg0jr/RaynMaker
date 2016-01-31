using System.Windows;
using System.Windows.Controls;
using RaynMaker.Modules.Import.Web.ViewModels;

namespace RaynMaker.Modules.Import.Web.Views
{
    public partial class DataSourcesTreeView : UserControl
    {
        public DataSourcesTreeView()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e )
        {
            ( ( DataSourcesTreeViewModel )DataContext ).SelectedItem = myTree.SelectedItem;
        }
    }
}
