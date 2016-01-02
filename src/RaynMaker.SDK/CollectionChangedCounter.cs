using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using Plainion;

namespace RaynMaker.SDK
{
    public class CollectionChangedCounter
    {
        public CollectionChangedCounter( IEnumerable source )
        {
            var observableSource = source as INotifyCollectionChanged;

            Contract.Requires( source != null, "source does not implement INotifyCollectionChanged" );

            Observe( observableSource );
        }

        private void Observe(INotifyCollectionChanged source)
        {
            WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>.AddHandler( source, "CollectionChanged", OnCollectionChanged );
        }

        public CollectionChangedCounter( INotifyCollectionChanged source )
        {
            Observe( source );
        }

        private void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            Count++;
        }

        public int Count { get; private set; }
    }
}
