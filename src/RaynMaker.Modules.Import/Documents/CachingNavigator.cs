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
            // TODO: key for caching would need include the locator with patterns + kind of hashcode from resolver
            // (e.g. including hash of all known macros)
            
            var key = macroResolver.CalculateLocationUID( locator );

            var uri = myCache.TryGet( key );
            if( uri == null )
            {
                uri = myNavigator.Navigate( locator, macroResolver );
                uri = myCache.Add( key, uri );
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
