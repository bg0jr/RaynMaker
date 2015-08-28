using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaynMaker.Import.Spec;
using Plainion.Logging;

namespace RaynMaker.Import.Core
{
    internal class DocumentBrowser : IDocumentBrowser
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( DocumentBrowser ) );
        
        private INavigator myNavigator;

        internal DocumentBrowser( INavigator navigator )
        {
            myNavigator = navigator;
        }

        public IDocument GetDocument( Navigation navi )
        {
            var uri = myNavigator.Navigate( navi );

            myLogger.Info( "Url from navigator: {0}", uri );

            var documentLoader = DocumentLoaderFactory.Create( navi.DocumentType );
            var doc = documentLoader.Load( uri );

            return doc;
        }
    }
}
