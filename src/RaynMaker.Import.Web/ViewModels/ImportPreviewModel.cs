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
using System.Data;

namespace RaynMaker.Import.Web.ViewModels
{
    class ImportPreviewModel : BindableBase, IBrowserBase
    {
        private StorageService myStorageService;
        private LegacyDocumentBrowser myDocumentBrowser = null;
        private Site mySelectedSite;
        private Type myDatumType;

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
            Data = Enumerable.Empty<IDatum>();
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

        public Site SelectedSite
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

            var data = new List<IDatum>();

            var provider = new BasicDatumProvider( this );

            var formats = mySelectedSite.Formats.Cast<PathSeriesFormat>();
            var format = formats.First();

            provider.Navigate( mySelectedSite, Stock );
            provider.Mark( format );

            var table = provider.GetResult( format );
            Currency currency = null; // TODO - how do we handle that!!
            foreach( DataRow row in table.Rows )
            {
                var year = ( int )row[ format.TimeAxisFormat.Name ];
                var value = ( double )row[ format.ValueFormat.Name ];

                var datum = Dynamics.CreateDatum( Stock, myDatumType, new YearPeriod( year ), currency );
                datum.Value = format.InMillions ? value * 1000000 : value;

                data.Add( datum );
            }

            Data = data;
        }

        public void Fetch( Type datum )
        {
            myDatumType = datum;

            var datumLocator = myStorageService.Load()
                .SingleOrDefault( l => l.Datum == myDatumType.Name );

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
