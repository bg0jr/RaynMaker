using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Entities.Datums;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Parsers.Html.WinForms;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Model;
using RaynMaker.Import.Web.Services;
using RaynMaker.Infrastructure;

namespace RaynMaker.Import.Web.ViewModels
{
    [Export]
    class WebSpyViewModel : BindableBase
    {
        private IDocumentBrowser myDocumentBrowser = null;

        [ImportingConstructor]
        public WebSpyViewModel( IProjectHost projectHost, StorageService storageService )
        {
            // TODO: caused by KeepAliveDelayedRegionCreationBehavior all the "heavy" logic below already runs when
            // starting the up - NOT when we open this window here
            // -> how could we get it lazy again to improve app startup performance?

            var session = new Session();

            foreach( var locator in storageService.Load() )
            {
                session.AddLocator( locator );
            }

            session.CurrentLocator = session.Locators.FirstOrDefault();

            Datums = new DatumSelectionViewModel( session );
            Navigation = new NavigationViewModel( session );
            Formats = new DataFormatsViewModel( session );
            Completion = new CompletionViewModel( session, projectHost, storageService );
        }

        public System.Windows.Forms.WebBrowser Browser
        {
            set
            {
                myDocumentBrowser = DocumentProcessorsFactory.CreateBrowser( value );

                // disable links
                // TODO: we cannot use this, it disables navigation in general (Navigate() too)
                //myBrowser.AllowNavigation = false;

                // TODO: how to disable images in browser

                Navigation.Browser = myDocumentBrowser;
                Formats.Browser = myDocumentBrowser;
                Completion.Browser = myDocumentBrowser;

                myDocumentBrowser.Navigate( DocumentType.Html, new Uri( "http://www.google.com" ) );
            }
        }

        //protected override void OnHandleDestroyed( EventArgs e )
        //{
        //    myMarkupDocument.Dispose();
        //    myMarkupDocument = null;

        //    myDocumentBrowser.Browser.DocumentCompleted -= myBrowser_DocumentCompleted;
        //    myDocumentBrowser.Dispose();
        //    myBrowser.Dispose();
        //    myBrowser = null;

        //    base.OnHandleDestroyed( e );
        //}

        public DatumSelectionViewModel Datums { get; private set; }

        public NavigationViewModel Navigation { get; private set; }

        public DataFormatsViewModel Formats { get; private set; }

        public CompletionViewModel Completion { get; private set; }
    }
}
