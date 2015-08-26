using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace RaynMaker.Entities
{
    /// <summary>
    /// Supports INotifyPropertyChanged for model entities.
    /// </summary>
    public abstract class EntityBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>( ref T storage, T value, [CallerMemberName] string propertyName = null )
        {
            if( object.Equals( storage, value ) )
            {
                return false;
            }

            storage = value;

            if( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }

            return true;
        }
    }
}
