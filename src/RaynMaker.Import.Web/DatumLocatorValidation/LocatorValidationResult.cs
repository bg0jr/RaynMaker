using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using RaynMaker.Import;
using System.ComponentModel;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.DatumLocatorValidation
{
    public class LocatorValidationResult
    {
        public static LocatorValidationResult Empty = new LocatorValidationResult();

        private LocatorValidationResult()
        {
            Result = null;
            DatumLocator = null;
            Navigation = Navigation.Empty;
        }

        public LocatorValidationResult( DataTable result, ParameterizedDatumLocator datumLocator, Navigation modifiedNavigation, string documentLocation, IFormat modifiedFormat )
        {
            Result = result;
            DatumLocator = datumLocator;
            Navigation = modifiedNavigation ?? Navigation.Empty;
            DocumentLocation = documentLocation;
            Format = modifiedFormat;
        }

        public DataTable Result { get; private set; }
        public ParameterizedDatumLocator DatumLocator { get; private set; }
        public Navigation Navigation { get; private set; }
        public string DocumentLocation { get; private set; }
        public IFormat Format { get; private set; }
        public string ErrorMessage { get; set; }

        public bool Succeeded { get { return Result != null; } }
    }
}
