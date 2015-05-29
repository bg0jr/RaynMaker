using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Infrastructure.Model;

namespace RaynMaker.Browser
{
    [Export]
    class BrowserViewModel : BindableBase
    {
        private Solution mySolution;

        [ImportingConstructor]
        public BrowserViewModel( Solution solution )
        {
            Contract.RequiresNotNull( solution, "solution" );

            mySolution = solution;

            WeakEventManager<ObservableCollection<IProject>, NotifyCollectionChangedEventArgs>
                .AddHandler( mySolution.Projects, "CollectionChanged", OnProjectsChanged );
        }

        private void OnProjectsChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            OnPropertyChanged( () => HasProject );
        }

        public bool HasProject { get { return mySolution.Projects.Any(); } }
    }
}
