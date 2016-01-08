using System;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Documents
{
    class CachingNavigator : INavigator, ICache, IDisposable
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

        public Uri Navigate( DocumentLocator locator, ILocatorMacroResolver macroResolver )
        {
            var uri = myCache.TryGet( locator );
            if( uri == null )
            {
                uri = myNavigator.Navigate( locator, macroResolver );
                uri = myCache.Add( locator, uri );
            }

            return uri;
        }

        public event Action<Uri> Navigating;

        public void Clear()
        {
            myCache.Clear();
        }
    }
}
