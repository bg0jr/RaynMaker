using System;
using System.Linq;
using Blade.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Entities;
using RaynMaker.Import.Core;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Html;
using RaynMaker.Import.Web.Services;
using Plainion;

namespace RaynMaker.Import.Web.ViewModels
{
    class ImportPreviewModel : BindableBase, IBrowserBase
    {
        private StorageService myStorageService;
        private LegacyDocumentBrowser myDocumentBrowser = null;
        private Site mySelectedSite;

        public ImportPreviewModel( StorageService storageService )
        {
            Contract.RequiresNotNull( storageService, "storageService" );

            myStorageService = storageService;

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );

            Sites = new ObservableCollection<Site>();
        }

        public Stock Stock { get; set; }

        public IPeriod From { get; set; }

        public IPeriod To { get; set; }

        public IEnumerable<IDatum> Data { get; set; }

        public Action FinishAction { get; set; }

        public ICommand OkCommand { get; private set; }

        private void OnOk()
        {
            FinishAction();
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishAction();
        }

        public System.Windows.Forms.WebBrowser Browser
        {
            set
            {
                myDocumentBrowser = new LegacyDocumentBrowser( value );

                if( SelectedSite != null )
                {
                    // we already got a call to fetch the data - just te browser was missing
                    // -> we have a browser now - lets fetch the data
                    TryFetch();
                }
            }
        }

        public ObservableCollection<Site> Sites { get; private set; }

        private Site SelectedSite
        {
            get { return mySelectedSite; }
            set
            {
                if( SetProperty( ref mySelectedSite, value ) )
                {
                    TryFetch();
                }
            }
        }

        private void TryFetch()
        {
            if( myDocumentBrowser == null )
            {
                return;
            }

            var provider = new BasicDatumProvider( this );

            var formats = mySelectedSite.Formats.Cast<PathSeriesFormat>();

            provider.Navigate( mySelectedSite, Stock );
            provider.Mark( formats.First() );

            var table = provider.GetResult( formats.First() );
        }

        public void Fetch( Type datum )
        {
            var datumLocator = myStorageService.Load()
                .SingleOrDefault( l => l.Datum == datum.Name );

            if( datumLocator == null || datumLocator.Sites.Count == 0 )
            {
                return;
            }

            Sites.AddRange( datumLocator.Sites.OrderBy( s => s.Name ) );
            SelectedSite = Sites.First();
        }

        public void Navigate( string url )
        {
            myDocumentBrowser.Navigate( url );
        }

        public IHtmlDocument LoadDocument( IEnumerable<NavigatorUrl> urls )
        {
            return myDocumentBrowser.LoadDocument( urls );
        }
    }
}
