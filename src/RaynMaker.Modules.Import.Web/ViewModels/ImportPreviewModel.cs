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
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Web.Services;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class ImportPreviewModel : BindableBase
    {
        private static readonly ILogger myLogger = LoggerFactory.GetLogger( typeof( ImportPreviewModel ) );

        private StorageService myStorageService;
        private IDocumentBrowser myDocumentBrowser = null;
        private DataSource mySelectedSource;
        private Type myDatumType;
        private List<IDatum> myData;
        private bool myOverwriteExistingValues;
        private Currency myCurrency;

        public ImportPreviewModel( StorageService storageService, ICurrenciesLut currenciesLut )
        {
            Contract.RequiresNotNull( storageService, "storageService" );
            Contract.RequiresNotNull( currenciesLut, "currenciesLut" );

            myStorageService = storageService;
            CurrenciesLut = currenciesLut;

            OkCommand = new DelegateCommand( OnOk );
            CancelCommand = new DelegateCommand( OnCancel );
            ApplyCommand = new DelegateCommand( OnApply );

            Sources = new ObservableCollection<DataSource>();
            myData = new List<IDatum>();
        }

        public ICurrenciesLut CurrenciesLut { get; private set; }

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

        public Currency Currency
        {
            get { return myCurrency; }
            set { SetProperty( ref myCurrency, value ); }
        }

        private void OnOk()
        {
            PublishData();
            FinishAction();
        }

        // only take over new datums and values for datums which have no value yet
        internal void PublishData()
        {
            foreach( var datum in myData )
            {
                if( datum.Period.CompareTo( From ) == -1 || datum.Period.CompareTo( To ) == 1 )
                {
                    continue;
                }

                var currencyDatum = datum as ICurrencyDatum;
                if( Currency != null && currencyDatum != null )
                {
                    ( ( AbstractCurrencyDatum )currencyDatum ).Currency = Currency;
                }

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

        public SafeWebBrowser Browser
        {
            set
            {
                myDocumentBrowser = DocumentProcessorsFactory.CreateBrowser( value );

                if( SelectedSource != null )
                {
                    // we already got a call to fetch the data - just te browser was missing
                    // -> we have a browser now - lets fetch the data
                    TryFetch();
                }
            }
        }

        public ObservableCollection<DataSource> Sources { get; private set; }

        public DataSource SelectedSource
        {
            get { return mySelectedSource; }
            set
            {
                if( SetProperty( ref mySelectedSource, value ) )
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

            var descriptors = mySelectedSource.Figures
                // TODO: only works as long as PathCellFormat is derived from PathSeriesFormat
                .Cast<PathSeriesDescriptor>()
                .Where( f => f.Figure == myDatumType.Name );

            foreach( var descriptor in descriptors )
            {
                try
                {
                    myDocumentBrowser.Navigate( DocumentType.Html, mySelectedSource.Location, new StockMacroResolver( Stock ) );
                }
                catch( Exception ex )
                {
                    ex.Data[ "Figure" ] = myDatumType.Name;
                    ex.Data[ "DataSource.Vendor" ] = mySelectedSource.Vendor;
                    ex.Data[ "DataSource.Name" ] = mySelectedSource.Name;
                    ex.Data[ "Location" ] = mySelectedSource.Location.ToString();

                    myLogger.Error( ex, "Failed to fetch '{0}' from site {1}", myDatumType.Name, mySelectedSource.Name );
                }

                // try take over currency
                //var pathCellFormat = format as PathCellDescriptor;
                //if( pathCellFormat != null )
                //{
                //    Currency = CurrenciesLut.Currencies.SingleOrDefault( c => c.Symbol == pathCellFormat.Currency );
                //}

                try
                {
                    var htmlDocument = ( IHtmlDocument )myDocumentBrowser.Document;

                    var marker = MarkupFactory.Create( descriptor );
                    marker.Mark( htmlDocument.GetElementByPath( HtmlPath.Parse( descriptor.Path ) ) );

                    // already extract data here to check for format issues etc

                    var parser = DocumentProcessorsFactory.CreateParser( htmlDocument, descriptor );
                    var table = parser.ExtractTable();
                    foreach( DataRow row in table.Rows )
                    {
                        if( row[ descriptor.ValueFormat.Name ] == DBNull.Value )
                        {
                            continue;
                        }

                        var value = ( double )row[ descriptor.ValueFormat.Name ];

                        IPeriod period;
                        if( descriptor.TimeFormat != null )
                        {
                            var year = ( int )row[ descriptor.TimeFormat.Name ];
                            period = new YearPeriod( year );
                        }
                        else
                        {
                            // TODO: is this a proper default?
                            period = new DayPeriod( DateTime.Now );
                        }

                        var datum = Dynamics.CreateDatum( Stock, myDatumType, period, null );
                        datum.Source = mySelectedSource.Vendor + " - " + mySelectedSource.Name;

                        datum.Value = descriptor.InMillions ? value * 1000000 : value;

                        myData.Add( datum );
                    }

                    // we found s.th. with this format 
                    // -> skip alternative formats
                    break;
                }
                catch( Exception ex )
                {
                    ex.Data[ "Figure" ] = myDatumType.Name;
                    ex.Data[ "DataSource.Vendor" ] = mySelectedSource.Vendor;
                    ex.Data[ "DataSource.Name" ] = mySelectedSource.Name;
                    ex.Data[ "Location" ] = mySelectedSource.Location.ToString();
                    ex.Data[ "FigureDescriptor" ] = descriptor.GetType().FullName;

                    myLogger.Error( ex, "Failed to fetch '{0}' from site {1}", myDatumType.Name, mySelectedSource.Name );
                }
            }
        }

        public void Fetch( Type datum )
        {
            myDatumType = datum;

            var sources = myStorageService.Load()
                .Where( source => source.Figures.Any( f => f.Figure == myDatumType.Name ) );

            Sources.AddRange( sources.OrderBy( s => s.Quality ) );
            SelectedSource = Sources.FirstOrDefault();
        }
    }
}
