using System;
using RaynMaker.Import;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.DatumLocatorValidation
{
    public class Validator
    {
        private IDocumentBrowser myWebScrapSC;

        public Validator( IDocumentBrowser webScrapSC )
        {
            myWebScrapSC = webScrapSC;
        }

        public LocatorValidationResult Fetch( ParameterizedDatumLocator datumLocator )
        {
            Navigation navigation = null;
            IDocument doc = null;
            IFormat format = null;

            try
            {
                var fetchPolicy = new EvaluatorPolicy( datumLocator.Parameters );
                navigation = fetchPolicy.GetNavigation( datumLocator.Site );

                doc = myWebScrapSC.GetDocument( navigation );
                if ( doc == null )
                {
                    return new LocatorValidationResult( null, datumLocator, navigation, null, null );
                }

                format = fetchPolicy.GetFormat( datumLocator.Site );
                var result = fetchPolicy.ApplyPreprocessing( doc.ExtractTable( format ) );

                var resultPolicy = new FirstNonNullPolicy();
                resultPolicy.Validate( datumLocator.Site, result );

                return new LocatorValidationResult( resultPolicy.ResultTable, datumLocator, navigation, doc.Location, format );
            }
            catch ( Exception ex )
            {
                var result =  new LocatorValidationResult( null, datumLocator, navigation, doc != null ? doc.Location : null, format );
                result.ErrorMessage = ex.Message;
                return result;
            }
        }
    }
}
