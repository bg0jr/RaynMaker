using System;
using Plainion.Logging;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.Documents
{
    class DocumentBrowser : IDocumentBrowser, IDisposable
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( DocumentBrowser ) );

        private INavigator myNavigator;

        public DocumentBrowser( INavigator navigator )
        {
            myNavigator = navigator;
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

                var disposable = myNavigator as IDisposable;
                if( disposable != null )
                {
                    disposable.Dispose();
                }

                myNavigator = null;
            }
        }

        public IDocument Document { get; private set; }

        public void Navigate( DocumentType docType, Uri url )
        {
            if( Navigating != null )
            {
                Navigating( url );
            }

            var documentLoader = DocumentLoaderFactory.CreateLoader( docType );
            Document = documentLoader.Load( url );

            if( DocumentCompleted != null )
            {
                DocumentCompleted( Document );
            }
        }

        public void Navigate( DocumentLocator navi )
        {
            var uri = myNavigator.Navigate( navi );

            myLogger.Info( "Url from navigator: {0}", uri );

            var documentLoader = DocumentLoaderFactory.CreateLoader( navi.DocumentType );
            Document = documentLoader.Load( uri );

            if( DocumentCompleted != null )
            {
                DocumentCompleted( Document );
            }
        }

        public event Action<Uri> Navigating;

        public event Action<IDocument> DocumentCompleted;

        public void ClearCache()
        {
            var cache = myNavigator as ICache;
            if( cache != null )
            {
                cache.Clear();
            }
        }
    }
}
