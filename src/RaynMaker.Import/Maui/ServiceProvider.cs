using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blade;

namespace Maui
{
    public class ServiceProvider
    {
        private Dictionary<string, object> myServices = null;

        public ServiceProvider()
        {
            myServices = new Dictionary<string, object>();
        }

        public object GetService( string key )
        {
            if( !myServices.ContainsKey( key ) )
            {
                throw new ArgumentException( "No such service: " + key );
            }

            return myServices[ key ];
        }

        public T GetServiceOrDefault<T>()
        {
            if( !myServices.ContainsKey( typeof( T ).ToString() ) )
            {
                return default( T );
            }

            return ( T )myServices[ typeof( T ).ToString() ];
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter" )]
        public T GetService<T>()
        {
            return ( T )GetService( typeof( T ).ToString() );
        }

        public IEnumerable<string> RegisteredServices
        {
            get { return myServices.Keys; }
        }

        public void RegisterService( string key, object service )
        {
            if( key == null )
            {
                throw new ArgumentNullException( "key" );
            }

            if( myServices.ContainsKey( key ) )
            {
                throw new ArgumentOutOfRangeException( "A service with key '" + key + "' is already registered " );
            }

            myServices[ key ] = service;
        }

        public void RegisterService( Type key, object service )
        {
            if( key == null )
            {
                throw new ArgumentNullException( "key" );
            }

            RegisterService( key.ToString(), service );
        }

        public void UnregisterService( string key )
        {
            if( key == null )
            {
                throw new ArgumentNullException( "key" );
            }

            myServices.Remove( key );
        }

        public void UnregisterService( Type key )
        {
            if( key == null )
            {
                throw new ArgumentNullException( "key" );
            }

            myServices.Remove( key.ToString() );
        }

        public void Reset()
        {
            foreach( var service in myServices.Values )
            {
                service.TryDispose();
            }
            myServices.Clear();
        }
    }
}
