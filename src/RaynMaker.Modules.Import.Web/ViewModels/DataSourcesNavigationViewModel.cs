using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Spec.v2.Locating;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class DataSourcesNavigationViewModel : SpecDefinitionViewModelBase, INotifyValidationFailed
    {
        private IEnumerable<DataSourceViewModel> mySources;
        private object mySelectedItem;

        public DataSourcesNavigationViewModel( Session session )
            : base( session )
        {
            AddDataSourceCommand = new DelegateCommand( OnAddDataSource );
            RemoveDataSourceCommand = new DelegateCommand( OnRemoveDataSource, CanRemoveDataSource );
            AddFigureCommand = new DelegateCommand( OnAddFigure, CanAddFigure );
            CopyFigureCommand = new DelegateCommand( OnCopyFigure, CanCopyFigure );
            RemoveFigureCommand = new DelegateCommand( OnRemoveFigure, CanRemoveFigure );

            DescriptorSelectionRequest = new InteractionRequest<FigureDescriptorSelectionNotification>();

            PropertyChangedEventManager.AddHandler( Session, OnSessionChanged, "" );

            CollectionChangedEventManager.AddHandler( Session.Sources, OnSourcesChanged );
            OnSourcesChanged( null, null );
        }

        private void OnSessionChanged( object sender, PropertyChangedEventArgs e )
        {
            if( e.PropertyName == PropertySupport.ExtractPropertyName( () => Session.CurrentFigureDescriptor ) )
            {
                var selectedVM = Sources
                    .Select( s => { s.IsSelected = false; return s; } )
                    .SelectMany( s => s.Figures )
                    .Select( f => { f.IsSelected = false; return f; } )
                    .SingleOrDefault( vm => vm.Model == Session.CurrentFigureDescriptor );

                if( selectedVM != null )
                {
                    selectedVM.IsSelected = true;
                }
            }
            else if( e.PropertyName == PropertySupport.ExtractPropertyName( () => Session.CurrentSource ) )
            {
                var selectedVM = Sources
                    .Select( s => { s.IsSelected = false; return s; } )
                    .SingleOrDefault( vm => vm.Model == Session.CurrentSource );

                if( selectedVM != null )
                {
                    selectedVM.IsSelected = true;
                }
            }
        }

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
            var notification = new FigureDescriptorSelectionNotification();
            notification.Title = "Figure descriptor selection";

            DescriptorSelectionRequest.Raise( notification, n =>
            {
                if( n.Confirmed )
                {
                    var descriptor = FigureDescriptorFactory.Create( n.DescriptorType );

                    // we can rely on the sessions current state as it is always in sync with the selection in the tree

                    Session.CurrentSource.Figures.Add( descriptor );

                    Session.CurrentFigureDescriptor = descriptor;
                }
            } );
        }

        public InteractionRequest<FigureDescriptorSelectionNotification> DescriptorSelectionRequest { get; private set; }

        private bool CanAddFigure()
        {
            return SelectedItem != null;
        }

        public DelegateCommand CopyFigureCommand { get; private set; }

        private void OnCopyFigure()
        {
            if( SelectedItem == null )
            {
                return;
            }

            // we can rely on the sessions current state as it is always in sync with the selection in the tree

            var descriptor = FigureDescriptorFactory.Clone( Session.CurrentFigureDescriptor );

            Session.CurrentSource.Figures.Add( descriptor );

            Session.CurrentFigureDescriptor = descriptor;
        }

        private bool CanCopyFigure()
        {
            return SelectedItem is FigureViewModel;
        }

        public DelegateCommand RemoveFigureCommand { get; private set; }

        private void OnRemoveFigure()
        {
            if( SelectedItem == null )
            {
                return;
            }

            // we can rely on the sessions current state as it is always in sync with the selection in the tree

            Session.CurrentSource.Figures.Remove( Session.CurrentFigureDescriptor );

            Session.CurrentFigureDescriptor = Session.CurrentSource.Figures.LastOrDefault();
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
                    CopyFigureCommand.RaiseCanExecuteChanged();
                    RemoveFigureCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public void FailedToLocateDocument( DataSource dataSource, string error )
        {
            var vm = Sources.Single( s => s.Model == dataSource );
            vm.ValidationResult = error;
        }

        public void NavigationSucceeded( DataSource dataSource )
        {
            var vm = Sources.Single( s => s.Model == dataSource );
            vm.ValidationResult = DataSourceViewModel.ValidationSucceeded;
        }

        public void FailedToParseDocument( IFigureDescriptor figureDescriptor, string error )
        {
            var vm = Sources
                .SelectMany( s => s.Figures )
                .Single( f => f.Model == figureDescriptor );
            vm.ValidationResult = error;
        }

        public void ParsingSucceeded( IFigureDescriptor figureDescriptor )
        {
            var vm = Sources
                .SelectMany( s => s.Figures )
                .Single( f => f.Model == figureDescriptor );
            vm.ValidationResult = DataSourceViewModel.ValidationSucceeded;
        }
    }
}
