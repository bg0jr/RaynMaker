using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using RaynMaker.Entities;
using RaynMaker.Import.Web.ViewModels;
using RaynMaker.Import.Web.Views;
using RaynMaker.Infrastructure.Services;

namespace RaynMaker.Import.Web
{
    [Export( typeof( IDataProvider ) )]
    class WebDatumProvider : IDataProvider
    {
        public IEnumerable<IDatum> Get( Stock stock, Type datum, IPeriod from, IPeriod to )
        {
            var previewViewModel = new ImportPreviewModel
            {
                Stock = stock,
                From = from,
                To = to
            };
            var preview = new ImportPreview { DataContext = previewViewModel };
            preview.ShowDialog();

            return previewViewModel.Data;
        }
    }
}
