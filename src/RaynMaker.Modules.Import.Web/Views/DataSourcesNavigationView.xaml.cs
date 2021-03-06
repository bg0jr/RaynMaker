﻿using System.Windows;
using System.Windows.Controls;
using RaynMaker.Modules.Import.Web.ViewModels;

namespace RaynMaker.Modules.Import.Web.Views
{
    public partial class DataSourcesNavigationView : UserControl
    {
        public DataSourcesNavigationView()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e )
        {
            ( ( DataSourcesNavigationViewModel )DataContext ).SelectedItem = myTree.SelectedItem;
        }
    }
}
