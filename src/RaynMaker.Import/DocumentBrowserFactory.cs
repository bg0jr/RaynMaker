using RaynMaker.Import.Core;

namespace RaynMaker.Import
{
    public class DocumentBrowserFactory
    {
        public static IDocumentBrowser Create()
        {
            return CreateEnhancedDocumentBrowser();
        }

        private static IDocumentBrowser CreateLegacyDocumentBrowser()
        {
            var browser = new WinFormsDocumentBrowser();

            browser.DownloadController.Options = BrowserOptions.NoActiveXDownload |
                BrowserOptions.NoBehaviors | BrowserOptions.NoJava | BrowserOptions.NoScripts |
                BrowserOptions.Utf8;

            return browser;
        }

        private static IDocumentBrowser CreateEnhancedDocumentBrowser()
        {
            var navigator = new CachingNavigator(
                new Navigator(),
                new DocumentCache() );

            var browser = new DocumentBrowser( navigator );

            return browser;
        }
    }
}
