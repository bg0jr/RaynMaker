using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using RaynMaker.Entities;
using RaynMaker.Import.Web.Services;
using RaynMaker.Import.Web.ViewModels;
using RaynMaker.Import.Web.Views;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Import.Web
{
    [Export( typeof( IDataProvider ) )]
    class WebDatumProvider : IDataProvider
    {
        private StorageService myStorageService;
        private ILutService myLutService;

        [ImportingConstructor]
        public WebDatumProvider( StorageService storageService, ILutService lutService )
        {
            myStorageService = storageService;
            myLutService = lutService;
        }

        public bool CanFetch( Type datum )
        {
            return myStorageService.Load()
                .Any( source => source.FormatSpecs.Any( f => f.Datum == datum.Name ) );
        }

        public void Fetch( DataProviderRequest request, ICollection<IDatum> resultContainer )
        {
            var previewViewModel = new ImportPreviewModel( myStorageService, myLutService.CurrenciesLut )
            {
                Stock = request.Stock,
                From = request.From,
                To = request.To,
                Series = resultContainer
            };

            if( request.WithPreview )
            {
                var preview = new ImportPreview( previewViewModel );

                previewViewModel.FinishAction = () => preview.Close();
                preview.DataContext = previewViewModel;

                previewViewModel.Fetch( request.DatumType );

                preview.Top = 0;
                preview.Left = 0;
                preview.Show();
            }
            else
            {
                var preview = new ImportPreview( previewViewModel );
                previewViewModel.Browser = new WinForms.SafeWebBrowser();
                previewViewModel.Fetch( request.DatumType );
                previewViewModel.PublishData();
            }
        }
    }
}
