using System;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Documents
{
    class CachingNavigator : INavigator
    {
        private INavigator myNavigator;
        private DocumentCache myCache;

        internal CachingNavigator( INavigator navigator, DocumentCache cache )
        {
            myNavigator = navigator;
            myCache = cache;
        }

        public Uri Navigate( Navigation navigation )
        {
            var uri = myCache.TryGet( navigation );
            if ( uri == null )
            {
                uri = myNavigator.Navigate( navigation );
                uri = myCache.Add( navigation, uri );
            }

            return uri;
        }
    }
}
