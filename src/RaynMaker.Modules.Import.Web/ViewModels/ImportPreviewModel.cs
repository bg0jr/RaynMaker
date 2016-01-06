using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                myDocumentBrowser = DocumentProcessingFactory.CreateBrowser( value );

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
                .Cast<IPathDescriptor>()
                .Where( f => f.Figure == myDatumType.Name );

            foreach( var descriptor in descriptors )
            {
                try
                {
                    myDocumentBrowser.Navigate( DocumentType.Html, mySelectedSource.Location, new StockMacroResolver( Stock ) );

                    var htmlDocument = ( IHtmlDocument )myDocumentBrowser.Document;

                    // Mark the part of the document described by the FigureDescriptor to have a preview

                    var marker = MarkupFactory.Create( descriptor );
                    marker.Mark( htmlDocument.GetElementByPath( HtmlPath.Parse( descriptor.Path ) ) );

                    // already extract data here to check for format issues etc

                    var parser = DocumentProcessingFactory.CreateParser( htmlDocument, descriptor );
                    var table = parser.ExtractTable();

                    var converter = DocumentProcessingFactory.CreateConverter( descriptor, mySelectedSource );
                    var series = converter.Convert( table, Stock );
                    myData.AddRange( series );

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
