using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Windows;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Spec.v2.Locating;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class DataSourcesNavigationViewModel : SpecDefinitionViewModelBase
    {
        private IEnumerable<DataSourceViewModel> mySources;
        private object mySelectedItem;

        public DataSourcesNavigationViewModel( Session session )
            : base( session )
        {
            AddDataSourceCommand = new DelegateCommand( OnAddDataSource );
            RemoveDataSourceCommand = new DelegateCommand( OnRemoveDataSource, CanRemoveDataSource );
            AddFigureCommand = new DelegateCommand( OnAddFigure, CanAddFigure );
            RemoveFigureCommand = new DelegateCommand( OnRemoveFigure, CanRemoveFigure );

            //PropertyChangedEventManager.AddHandler( Session, OnSessionChanged, "" );

            CollectionChangedEventManager.AddHandler( Session.Sources, OnSourcesChanged );
            OnSourcesChanged( null, null );
        }

        // TODO: this code breakes user selection
        //private void OnSessionChanged( object sender, PropertyChangedEventArgs e )
        //{
        //    if( e.PropertyName == PropertySupport.ExtractPropertyName( () => Session.CurrentFigureDescriptor ) )
        //    {
        //        var selectedVM = Sources
        //            .SelectMany( s => s.Figures )
        //            .Single( vm => vm.Model == Session.CurrentFigureDescriptor );
        //        selectedVM.IsSelected = true;
        //    }
        //    else if( e.PropertyName == PropertySupport.ExtractPropertyName( () => Session.CurrentSource ) )
        //    {
        //        var selectedVM = Sources.Single( vm => vm.Model == Session.CurrentSource );
        //        selectedVM.IsSelected = true;
        //    }
        //}

        private void OnSourcesChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            Sources = Session.Sources
                .Select( s => new DataSourceViewModel( Session, s ) )
                .ToList();
        }

        public IEnumerable<DataSourceViewModel> Sources
        {
            get { return mySources; }
            private set { SetProperty( ref mySources, value ); }
        }

        public ICommand AddDataSourceCommand { get; private set; }

        private void OnAddDataSource()
        {
            var source = new DataSource();
            source.Location = new DocumentLocator();

            Session.Sources.Add( source );

            Session.CurrentSource = source;
        }

        public DelegateCommand RemoveDataSourceCommand { get; private set; }

        private void OnRemoveDataSource()
        {
            var source = ( ( DataSourceViewModel )SelectedItem ).Model;
            Session.Sources.Remove( source );
            Session.CurrentSource = Session.Sources.FirstOrDefault();
        }

        private bool CanRemoveDataSource()
        {
            return SelectedItem != null;
        }

        public DelegateCommand AddFigureCommand { get; private set; }

        private void OnAddFigure()
        {
        }

        private bool CanAddFigure()
        {
            return SelectedItem != null;
        }

        public DelegateCommand RemoveFigureCommand { get; private set; }

        private void OnRemoveFigure()
        {
        }

        private bool CanRemoveFigure()
        {
            return SelectedItem is FigureViewModel;
        }

        public object SelectedItem
        {
            get { return mySelectedItem; }
            set
            {
                if( SetProperty( ref mySelectedItem, value ) )
                {
                    // do NOT update the Model or the TreeViewItem ViewModels.
                    // both get updated automatically through IsSelected binding.
                    // -> use as "listener" only

                    RemoveDataSourceCommand.RaiseCanExecuteChanged();
                    AddFigureCommand.RaiseCanExecuteChanged();
                    RemoveFigureCommand.RaiseCanExecuteChanged();
                }
            }
        }
    }
}
