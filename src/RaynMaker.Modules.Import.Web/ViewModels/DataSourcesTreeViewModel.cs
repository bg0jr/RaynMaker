using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class DataSourcesTreeViewModel : SpecDefinitionViewModelBase
    {
        private IEnumerable<DataSourceViewModel> mySources;

        public DataSourcesTreeViewModel( Session session )
            : base( session )
        {
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
    }
}
