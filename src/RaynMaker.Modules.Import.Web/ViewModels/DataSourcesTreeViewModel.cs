using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class DataSourcesTreeViewModel : SpecDefinitionViewModelBase
    {
        private IEnumerable<DataSourceViewModel> mySources;
        private string myAddCaption;
        private string myRemoveCaption;
        private object mySelectedItem;

        public DataSourcesTreeViewModel( Session session )
            : base( session )
        {
            AddCommand = new DelegateCommand( OnAdd );
            RemoveCommand = new DelegateCommand( OnRemove, CanRemove );

            UpdateButtonText();

            CollectionChangedEventManager.AddHandler( Session.Sources, OnSourcesChanged );
            OnSourcesChanged( null, null );
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

        public ICommand AddCommand { get; private set; }

        private void OnAdd()
        {
        }

        public DelegateCommand RemoveCommand { get; private set; }

        private void OnRemove()
        {
        }

        private bool CanRemove()
        {
            return SelectedItem != null;
        }

        public string AddCaption
        {
            get { return myAddCaption; }
            set { SetProperty( ref myAddCaption, value ); }
        }

        public string RemoveCaption
        {
            get { return myRemoveCaption; }
            set { SetProperty( ref myRemoveCaption, value ); }
        }

        public object SelectedItem
        {
            get { return mySelectedItem; }
            set
            {
                if( SetProperty( ref mySelectedItem, value ) )
                {
                    UpdateButtonText();

                    RemoveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private void UpdateButtonText()
        {
            AddCaption = "Add " + ( mySelectedItem == null || mySelectedItem is DataSourceViewModel ? "DataSource" : "Figure" );
            RemoveCaption = "Remove " + ( mySelectedItem == null || mySelectedItem is DataSourceViewModel ? "DataSource" : "Figure" );
        }
    }
}
