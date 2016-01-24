using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using RaynMaker.Entities;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Web.Services;
using RaynMaker.Modules.Import.Web.ViewModels;
using RaynMaker.Modules.Import.Web.Views;

namespace RaynMaker.Modules.Import.Web
{
    [Export( typeof( IDataProvider ) )]
    class WebFigureProvider : IDataProvider
    {
        private StorageService myStorageService;
        private ILutService myLutService;

        // for usability reasons we remember the currency settings for the stock
        private CurrencyCache myCurrencyCache;

        private class CurrencyCache
        {
            public string Isin;
            public Currency Currency;
        }

        [ImportingConstructor]
        public WebFigureProvider( StorageService storageService, ILutService lutService )
        {
            myStorageService = storageService;
            myLutService = lutService;
            myCurrencyCache = new CurrencyCache();
        }

        public bool CanFetch( Type figure )
        {
            return myStorageService.Load()
                .Any( source => source.Figures.Any( f => f.Figure == figure.Name ) );
        }

        // TODO: actually we would like to have this in the request but this does not work because of 
        // dependencies from Infrastructure to import module
        public Func<ILocatorMacroResolver, ILocatorMacroResolver> CustomResolverCreator { get; set; }
        
        public void Fetch( DataProviderRequest request, ICollection<IFigure> resultContainer )
        {
            var previewViewModel = new ImportPreviewModel( myStorageService, myLutService.CurrenciesLut )
            {
                Stock = request.Stock,
                From = request.From,
                To = request.To,
                Series = resultContainer,
                ThrowOnError = request.ThrowOnError,
                CustomResolverCreator = CustomResolverCreator
            };

            if( request.WithPreview )
            {
                if( myCurrencyCache.Isin == request.Stock.Isin )
                {
                    // take over last setting from user
                    previewViewModel.Currency = myCurrencyCache.Currency;
                }

                var preview = new ImportPreview( previewViewModel );

                previewViewModel.FinishAction = () =>
                {
                    preview.Close();

                    // remember last setting from user
                    if( previewViewModel.Currency != null )
                    {
                        myCurrencyCache.Isin = previewViewModel.Stock.Isin;
                        myCurrencyCache.Currency = previewViewModel.Currency;
                    }
                };
                preview.DataContext = previewViewModel;

                previewViewModel.Fetch( request.FigureType );

                preview.Top = 0;
                preview.Left = 0;
                preview.Show();
            }
            else
            {
                previewViewModel.Browser = new SafeWebBrowser();
                previewViewModel.Fetch( request.FigureType );
                previewViewModel.PublishData();
            }
        }
    }
}
