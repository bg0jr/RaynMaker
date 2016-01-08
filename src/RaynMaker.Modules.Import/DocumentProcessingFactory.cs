using System;
using System.Collections.Generic;
using System.Linq;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Converters;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Parsers.Text;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import
{
    public static class DocumentProcessingFactory
    {
        public static IDocumentBrowser CreateBrowser()
        {
            return new DocumentBrowser( CreateNavigator() );
        }

        private static CachingNavigator CreateNavigator()
        {
            var navigator = new CachingNavigator(
                new Navigator(),
                new DocumentCache() );
            return navigator;
        }

        public static IDocumentBrowser CreateBrowser( SafeWebBrowser webBrowser )
        {
            return new WinFormsDocumentBrowser( CreateNavigator(), webBrowser );
        }

        public static IDocumentParser CreateParser( IDocument document, IFigureDescriptor descriptor )
        {
            Contract.RequiresNotNull( document, "document" );
            Contract.RequiresNotNull( descriptor, "descriptor" );

            var htmlDocument = document as IHtmlDocument;
            if( htmlDocument != null )
            {
                return new HtmlParser( htmlDocument, descriptor );
            }

            var textDocument = document as TextDocument;
            if( textDocument != null )
            {
                return new TextParser( textDocument, descriptor );
            }

            throw new NotSupportedException( "Unable to find parser for document type: " + document.GetType() );
        }

        public static IDataTableToEntityConverter CreateConverter( IFigureDescriptor descriptor, DataSource dataSource, IEnumerable<Currency> currencies )
        {
            Contract.RequiresNotNull( descriptor, "descriptor" );

            var entityType = Dynamics.AllDatums.SingleOrDefault( f => f.Name == descriptor.Figure );

            Contract.Requires( entityType != null, "No entity of type {0} found", descriptor.Figure );

            var source = dataSource.Vendor + "|" + dataSource.Name;

            if ( descriptor is SeriesDescriptorBase ) return new DataTableToSeriesConverter( descriptor as SeriesDescriptorBase, entityType, source, currencies );
            if ( descriptor is SingleValueDescriptorBase ) return new DataTableToSingleValueConverter( descriptor as SingleValueDescriptorBase, entityType, source, currencies );
            //if( descriptor is TableDescriptorBase ) return new DataTableToTableConverter( descriptor as TableDescriptorBase, entityType, source );

            throw new NotSupportedException( "Unknown descriptor type: " + descriptor.GetType().Name );
        }
    }
}
