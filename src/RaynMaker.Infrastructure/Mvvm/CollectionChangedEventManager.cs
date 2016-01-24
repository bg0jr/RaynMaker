using System;
using System.Collections.Specialized;
using System.Windows;

namespace RaynMaker.Infrastructure.Mvvm
{
    /// <summary>
    /// Wrapper around WeakEventManager for INotifyCollectionChanged
    /// </summary>
    public static class CollectionChangedEventManager
    {
        public static void AddHandler( INotifyCollectionChanged source, EventHandler<NotifyCollectionChangedEventArgs> handler )
        {
            WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>.AddHandler( source, "CollectionChanged", handler );
        }

        public static void RemoveHandler( INotifyCollectionChanged source, EventHandler<NotifyCollectionChangedEventArgs> handler )
        {
            WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>.RemoveHandler( source, "CollectionChanged", handler );
        }
    }
}
