using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Blade
{
    /// <summary>
    /// Extentions to System.Object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Disposes the given object if it implements System.IDisposable.
        /// </summary>
        public static void TryDispose( this object obj )
        {
            IDisposable disposable = obj as IDisposable;
            if( disposable != null )
            {
                disposable.Dispose();
            }
        }
        /// <summary>
        /// Disposes the given object if it is not null.
        /// </summary>
        public static void TryDispose( this IDisposable obj )
        {
            if( obj != null )
            {
                obj.Dispose();
            }
        }
        /// <summary>
        /// Generic deep clone using serializers.
        /// </summary>
        public static T Clone<T>( this T source )
        {
            if( !typeof( T ).IsSerializable )
            {
                throw new ArgumentException( "The type must be serializable.", "source" );
            }
            if( object.ReferenceEquals( source, null ) )
            {
                return default( T );
            }
            IFormatter formatter = new BinaryFormatter();
            T result;
            using( MemoryStream memoryStream = new MemoryStream() )
            {
                formatter.Serialize( memoryStream, source );
                memoryStream.Seek( 0L, SeekOrigin.Begin );
                result = ( T )( ( object )formatter.Deserialize( memoryStream ) );
            }
            return result;
        }
        /// <summary>
        /// Generic hashcode generation for object graphs using serializers.
        /// </summary>
        public static int GetDeepHashCode( this object source )
        {
            if( !source.GetType().IsSerializable )
            {
                throw new ArgumentException( "The type must be serializable.", "source" );
            }
            if( object.ReferenceEquals( source, null ) )
            {
                return 0;
            }
            IFormatter formatter = new BinaryFormatter();
            int hashCode;
            using( MemoryStream memoryStream = new MemoryStream() )
            {
                formatter.Serialize( memoryStream, source );
                hashCode = Encoding.Unicode.GetString( memoryStream.ToArray() ).GetHashCode();
            }
            return hashCode;
        }
    }
}
