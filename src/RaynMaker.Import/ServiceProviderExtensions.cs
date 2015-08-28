using System.Linq;
using Maui;
using RaynMaker.Import.Core;

namespace RaynMaker.Import
{
    public static class ServiceProviderExtensions
    {
        public static IDocumentBrowser Browser( this ServiceProvider serviceProvider )
        {
            if ( !serviceProvider.RegisteredServices.Contains( typeof( IDocumentBrowser ).ToString() ) )
            {
                var browser = CreateEnhancedDocumentBrowser( serviceProvider );
                serviceProvider.RegisterService( typeof( IDocumentBrowser ), browser );
            }

            return serviceProvider.GetService<IDocumentBrowser>();
        }

        private static IDocumentBrowser CreateLegacyDocumentBrowser( ServiceProvider serviceProvider )
        {
            var browser = new LegacyDocumentBrowser();
            browser.Init( serviceProvider );

            browser.DownloadController.Options = BrowserOptions.NoActiveXDownload |
                BrowserOptions.NoBehaviors | BrowserOptions.NoJava | BrowserOptions.NoScripts |
                BrowserOptions.Utf8;

            return browser;
        }

        private static IDocumentBrowser CreateEnhancedDocumentBrowser( ServiceProvider serviceProvider )
        {
            var navigator = new CachingNavigator(
                new Navigator(),
                new DocumentCache() );

            var browser = new DocumentBrowser( navigator );

            return browser;
        }
    }
}
