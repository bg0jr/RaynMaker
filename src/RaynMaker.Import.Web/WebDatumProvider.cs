using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using RaynMaker.Entities;
using RaynMaker.Import.Web.Services;
using RaynMaker.Import.Web.ViewModels;
using RaynMaker.Import.Web.Views;
using System.Linq;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Import.Web
{
    [Export( typeof( IDataProvider ) )]
    class WebDatumProvider : IDataProvider
    {
        private StorageService myStorageService;

        [ImportingConstructor]
        public WebDatumProvider( StorageService storageService )
        {
            myStorageService = storageService;
        }

        public void Fetch( Stock stock, Type datum, ICollection<IDatum> series, IPeriod from, IPeriod to )
        {
            var previewViewModel = new ImportPreviewModel(myStorageService)
            {
                Stock = stock,
                From = from,
                To = to,
                Series = series
            };

            var preview = new ImportPreview( previewViewModel );

            previewViewModel.FinishAction = () => preview.Close();
            preview.DataContext = previewViewModel;

            previewViewModel.Fetch( datum );

            preview.Show();
        }
    }
}
