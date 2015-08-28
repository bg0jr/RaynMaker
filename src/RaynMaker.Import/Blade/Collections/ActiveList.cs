using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Collections
{
    /// <summary>
    /// Implements a list which allows to react on modification of the list.
    /// <remarks>
    /// The list is serializable but the registered delegates (insertion, removal)
    /// will not be serialized.
    /// </remarks>
    /// </summary>
    [Serializable]
    public class ActiveList<T> : IList<T>, ICollection<T>, IArray<T>, IEnumerable<T>, IEnumerable
    {
        private List<T> myList;
        /// <summary>
        /// Called when an item will be inserted or added.
        /// </summary>
        [NonSerialized]
        public Action<T, int> ItemInserting;
        /// <summary>
        /// Called when an item has been inserted or added.
        /// </summary>
        [NonSerialized]
        public Action<T, int> ItemInserted;
        /// <summary>
        /// Called when an item will be removed.
        /// </summary>
        [NonSerialized]
        public Action<T> ItemRemoving;
        /// <summary>
        /// Called when an item has been removed.
        /// </summary>
        [NonSerialized]
        public Action<T> ItemRemoved;
        /// <see cref="T:IList.[]" />
        public T this[ int index ]
        {
            get
            {
                return this.myList[ index ];
            }
            set
            {
                this.myList[ index ] = value;
            }
        }
        /// <see cref="T:ICollection.Count" />
        public int Count
        {
            get
            {
                return this.myList.Count;
            }
        }
        /// <see cref="T:ICollection.IsReadOnly" />
        public bool IsReadOnly
        {
            get
            {
                return ( ( ICollection<T> )this.myList ).IsReadOnly;
            }
        }
        /// <summary>
        /// Creates a copy of the given set.
        /// </summary>
        public ActiveList( IEnumerable<T> list )
        {
            this.myList = ( ( list != null ) ? new List<T>( list ) : new List<T>() );
        }
        /// <summary>
        /// Creates a copy of the given set.
        /// </summary>
        public ActiveList( params T[] items )
        {
            this.myList = ( ( items != null ) ? new List<T>( items ) : new List<T>() );
        }
        /// <summary>
        /// Creates a new empty list.
        /// </summary>
        public ActiveList()
        {
            this.myList = new List<T>();
        }
        /// <see cref="T:IList.IndexOf()" />
        public int IndexOf( T item )
        {
            return this.myList.IndexOf( item );
        }
        /// <see cref="T:IList.Insert()" />
        public void Insert( int index, T item )
        {
            if( this.ItemInserting != null )
            {
                this.ItemInserting( item, index );
            }
            this.myList.Insert( index, item );
            if( this.ItemInserted != null )
            {
                this.ItemInserted( item, index );
            }
        }
        /// <see cref="T:IList.RemoveAt()" />
        public void RemoveAt( int index )
        {
            if( this.ItemRemoving != null )
            {
                this.ItemRemoving( this[ index ] );
            }
            this.myList.RemoveAt( index );
            if( this.ItemRemoved != null )
            {
                this.ItemRemoved( this[ index ] );
            }
        }
        /// <see cref="T:ICollection.Add" />
        public void Add( T item )
        {
            if( this.ItemInserting != null )
            {
                this.ItemInserting( item, this.myList.Count );
            }
            this.myList.Add( item );
            if( this.ItemInserted != null )
            {
                this.ItemInserted( item, this.myList.Count - 1 );
            }
        }
        /// <see cref="T:ICollection.Clear" />
        public void Clear()
        {
            while( this.myList.Count > 0 )
            {
                this.RemoveAt( 0 );
            }
        }
        /// <see cref="T:ICollection.Contains" />
        public bool Contains( T item )
        {
            return this.myList.Contains( item );
        }
        /// <see cref="T:ICollection.CopyTo" />
        public void CopyTo( T[] array, int arrayIndex )
        {
            this.myList.CopyTo( array, arrayIndex );
        }
        /// <see cref="T:ICollection.Remove" />
        public bool Remove( T item )
        {
            if( this.ItemRemoving != null )
            {
                this.ItemRemoving( item );
            }
            bool result = this.myList.Remove( item );
            if( this.ItemRemoved != null )
            {
                this.ItemRemoved( item );
            }
            return result;
        }
        /// <see cref="T:IEnumerable.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            return this.myList.GetEnumerator();
        }
        /// <see cref="M:System.Collections.IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.myList.GetEnumerator();
        }
    }
}
