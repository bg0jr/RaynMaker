using System.Collections.Generic;
using RaynMaker.Entities;

namespace RaynMaker.Import.Web.ViewModels
{
    class ImportPreviewModel
    {
        public ImportPreviewModel()
        {
        }

        public Stock Stock { get; set; }

        public IPeriod From { get; set; }
        
        public IPeriod To { get; set; }

        public IEnumerable<IDatum> Data { get; set; }
    }
}
