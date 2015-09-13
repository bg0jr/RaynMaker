using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using Plainion.Collections;
using Plainion.Logging;
using RaynMaker.Entities;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.Services;

namespace RaynMaker.Import.Web.ViewModels
{
    class ImportPreviewModel : BindableBase, IBrowserBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( ImportPreviewModel ) );

        private StorageService myStorageService;
        private IDocumentBrowser myDocumentBrowser = null;
        private Site mySelectedSite;
        private Type myDatumType;
        private List<IDatum> myData;
        private bool myOverwriteExistingValues;

        public ImportPreviewModel( StorageService storageService )
        {
            Contract.RequiresNotNull( storageService, "storageService" );

            myStorageService = storageService;

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
            ApplyCommand = new DelegateCommand( OnApply );

            Sites = new ObservableCollection<Site>();
            myData = new List<IDatum>();
        }

        public Stock Stock { get; set; }

        public IPeriod From { get; set; }

        public IPeriod To { get; set; }

        public ICollection<IDatum> Series { get; set; }

        public Action FinishAction { get; set; }

        public ICommand OkCommand { get; private set; }

        public bool OverwriteExistingValues
        {
            get { return myOverwriteExistingValues; }
            set { SetProperty( ref myOverwriteExistingValues, value ); }
        }

        private void OnOk()
        {
            PublishData();
            FinishAction();
        }

        // only take over new datums and values for datums which have no value yet
        private void PublishData()
        {
            foreach( var datum in myData )
            {
                var existingDatum = Series.SingleOrDefault( d => d.Period.Equals( datum.Period ) );
                if( existingDatum == null )
                {
                    Series.Add( datum );
                    continue;
                }

                if( !existingDatum.Value.HasValue || OverwriteExistingValues )
                {
                    ( ( AbstractDatum )existingDatum ).Value = datum.Value;
                    ( ( AbstractDatum )existingDatum ).Source = datum.Source;

                    var currencyDatum = datum as ICurrencyDatum;
                    if( currencyDatum != null )
                    {
                        ( ( AbstractCurrencyDatum )existingDatum ).Currency = currencyDatum.Currency;
                    }
                }
            }
        }

        public ICommand CancelCommand { get; private set; }

        private void OnCancel()
        {
            FinishAction();
        }

        public ICommand ApplyCommand { get; private set; }

        private void OnApply()
        {
            PublishData();
        }

        public System.Windows.Forms.WebBrowser Browser
        {
            set
            {
                myDocumentBrowser = DocumentProcessorsFactory.CreateBrowser(value );

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

            myData.Clear();

            var provider = new BasicDatumProvider( this );

            var formats = mySelectedSite.Formats.Cast<PathSeriesFormat>();

            foreach( var format in formats )
            {
                try
                {
                    provider.Navigate( mySelectedSite, Stock );
                }
                catch( Exception ex )
                {
                    ex.Data[ "Datum" ] = myDatumType.Name;
                    ex.Data[ "SiteName" ] = mySelectedSite.Name;
                    ex.Data[ "OriginalNavigation" ] = mySelectedSite.Navigation;
                    //ex.Data[ "ModifiedNavigation" ] = modifiedNavigation;

                    myLogger.Error( ex, "Failed to fetch '{0}' from site {1}", myDatumType.Name, mySelectedSite.Name );
                }

                try
                {
                    provider.Mark( format );

                    // already extract data here to check for format issues etc

                    var table = provider.GetResult( format );
                    Currency currency = null; // TODO - how do we handle that!!
                    foreach( DataRow row in table.Rows )
                    {
                        var year = ( int )row[ format.TimeAxisFormat.Name ];
                        var value = ( double )row[ format.ValueFormat.Name ];

                        var datum = Dynamics.CreateDatum( Stock, myDatumType, new YearPeriod( year ), currency );
                        datum.Source = mySelectedSite.Name;

                        datum.Value = format.InMillions ? value * 1000000 : value;

                        myData.Add( datum );
                    }

                    // we found s.th. with this format 
                    // -> skip alternative formats
                    break;
                }
                catch( Exception ex )
                {
                    ex.Data[ "Datum" ] = myDatumType.Name;
                    ex.Data[ "SiteName" ] = mySelectedSite.Name;
                    ex.Data[ "OriginalNavigation" ] = mySelectedSite.Navigation;
                    ex.Data[ "OriginalFormat" ] = format;

                    myLogger.Error( ex, "Failed to fetch '{0}' from site {1}", myDatumType.Name, mySelectedSite.Name );
                }
            }
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
            myDocumentBrowser.Navigate( DocumentType.Html, new Uri( url ) );
        }

        public IHtmlDocument LoadDocument( IEnumerable<NavigatorUrl> urls )
        {
            myDocumentBrowser.Navigate( new Navigation( DocumentType.Html, urls ) );
            return ( ( HtmlDocumentHandle )myDocumentBrowser.Document ).Content;
        }
    }
}
