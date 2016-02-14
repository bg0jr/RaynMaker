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
using RaynMaker.Modules.Import.Documents.WinForms;
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
        private Type myFigureType;
        private List<IFigure> myData;
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
            myData = new List<IFigure>();
        }

        public ICurrenciesLut CurrenciesLut { get; private set; }

        public Stock Stock { get; set; }

        public IPeriod From { get; set; }

        public IPeriod To { get; set; }

        public ICollection<IFigure> Series { get; set; }

        public bool ThrowOnError { get; set; }

        public Func<ILocatorMacroResolver, ILocatorMacroResolver> CustomResolverCreator { get; set; }

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

        // only take over new figures and values for figures which have no value yet
        internal void PublishData()
        {
            foreach( var figure in myData )
            {
                if( figure.Period.CompareTo( From ) == -1 || figure.Period.CompareTo( To ) == 1 )
                {
                    continue;
                }

                var currencyFigure = figure as ICurrencyFigure;
                if( Currency != null && currencyFigure != null )
                {
                    ( ( AbstractCurrencyFigure )currencyFigure ).Currency = Currency;
                }

                var existingFigure = Series.SingleOrDefault( d => d.Period.Equals( figure.Period ) );
                if( existingFigure == null )
                {
                    Series.Add( figure );
                    continue;
                }

                if( !existingFigure.Value.HasValue || OverwriteExistingValues )
                {
                    ( ( AbstractFigure )existingFigure ).Value = figure.Value;
                    ( ( AbstractFigure )existingFigure ).Source = figure.Source;

                    if( currencyFigure != null )
                    {
                        ( ( AbstractCurrencyFigure )existingFigure ).Currency = currencyFigure.Currency;
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
                .Where( f => f.Figure == myFigureType.Name );

            foreach( var descriptor in descriptors )
            {
                try
                {
                    ILocatorMacroResolver resolver = new StockMacroResolver( Stock );
                    if( CustomResolverCreator != null )
                    {
                        resolver = CustomResolverCreator( resolver );
                    }
                    myDocumentBrowser.Navigate( DocumentType.Html, mySelectedSource.Location, resolver );

                    var htmlDocument = ( IHtmlDocument )myDocumentBrowser.Document;

                    // Mark the part of the document described by the FigureDescriptor to have a preview

                    var cell = ( HtmlElementAdapter )MarkupFactory.FindElementByDescriptor( htmlDocument, descriptor );
                    if( cell != null )
                    {
                        cell.Element.ScrollIntoView( false );
                    }

                    var marker = MarkupFactory.CreateMarker( descriptor );
                    marker.Mark( cell );

                    // already extract data here to check for format issues etc

                    var parser = DocumentProcessingFactory.CreateParser( htmlDocument, descriptor );
                    var table = parser.ExtractTable();

                    var converter = DocumentProcessingFactory.CreateConverter( descriptor, mySelectedSource, CurrenciesLut.Currencies );
                    var series = converter.Convert( table, Stock );
                    myData.AddRange( series );

                    // we found s.th. with this format 
                    // -> skip alternative formats
                    break;
                }
                catch( Exception ex )
                {
                    ex.Data[ "Figure" ] = myFigureType.Name;
                    ex.Data[ "DataSource.Vendor" ] = mySelectedSource.Vendor;
                    ex.Data[ "DataSource.Name" ] = mySelectedSource.Name;
                    ex.Data[ "Location" ] = mySelectedSource.Location.ToString();
                    ex.Data[ "FigureDescriptor" ] = descriptor.GetType().FullName;

                    if( ThrowOnError )
                    {
                        throw new Exception( "Failed to extract data from datasource", ex );
                    }
                    else
                    {
                        myLogger.Error( ex, "Failed to fetch '{0}' from site {1}", myFigureType.Name, mySelectedSource.Name );
                    }
                }
            }
        }

        public void Fetch( Type figure )
        {
            myFigureType = figure;

            var sources = myStorageService.Load()
                .Where( source => source.Figures.Any( f => f.Figure == myFigureType.Name ) );

            Sources.AddRange( sources.OrderBy( s => s.Quality ) );
            SelectedSource = Sources.FirstOrDefault();
        }
    }
}
