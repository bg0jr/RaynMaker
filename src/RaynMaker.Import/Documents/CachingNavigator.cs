using System;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Documents
{
    class CachingNavigator : INavigator, IDisposable
    {
        private INavigator myNavigator;
        private DocumentCache myCache;

        public CachingNavigator( INavigator navigator, DocumentCache cache )
        {
            myNavigator = navigator;
            myCache = cache;

            myNavigator.Navigating += OnNavigating;
        }

        private void OnNavigating( Uri url )
        {
            if( Navigating != null )
            {
                Navigating( url );
            }
        }

        public void Dispose()
        {
            if( myNavigator != null )
            {
                myNavigator.Navigating -= OnNavigating;
                myNavigator = null;
            }
        }

        public Uri Navigate( Navigation navigation )
        {
            var uri = myCache.TryGet( navigation );
            if( uri == null )
            {
                uri = myNavigator.Navigate( navigation );
                uri = myCache.Add( navigation, uri );
            }

            return uri;
        }

        public event Action<Uri> Navigating;
    }
}
