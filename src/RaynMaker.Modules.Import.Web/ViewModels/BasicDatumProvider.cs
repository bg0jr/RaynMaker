using System.Data;
using System.Linq;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class BasicDatumProvider
    {
        private IDocumentBrowser myBrowser;

        public BasicDatumProvider( IDocumentBrowser browser )
        {
            myBrowser = browser;
        }

        public IHtmlDocument Document { get; private set; }

        public void Navigate( DocumentType docType, DocumentLocator locator, Stock stock )
        {
            Contract.RequiresNotNull( locator, "navigation" );
            Contract.Requires( docType == DocumentType.Html, "Only DocumentType.Html supported" );
            Contract.RequiresNotNull( stock, "stock" );

            myBrowser.Navigate( docType, locator, new StockMacroResolver( stock ) );
            Document = ( IHtmlDocument )myBrowser.Document;
        }

        public void Mark( PathSeriesDescriptor format )
        {
            Contract.RequiresNotNull( format, "format" );
            Contract.Invariant( Document != null, "Document not yet loaded" );

            var markupBehavior = new HtmlMarkupBehavior<HtmlTableMarker>( new HtmlTableMarker() );
            markupBehavior.AttachTo( ( HtmlDocumentAdapter )Document );
            markupBehavior.PathToSelectedElement = format.Path;

            if( format.Orientation == SeriesOrientation.Row )
            {
                markupBehavior.Marker.ExpandRow = true;

                markupBehavior.Marker.ColumnHeaderRow = format.TimesLocator.HeaderSeriesPosition;
                markupBehavior.Marker.RowHeaderColumn = format.ValuesLocator.HeaderSeriesPosition;

                markupBehavior.Marker.SkipColumns = null;
                markupBehavior.Marker.SkipRows = format.Excludes.ToArray();
            }
            else if( format.Orientation == SeriesOrientation.Column )
            {
                markupBehavior.Marker.ExpandColumn = true;

                markupBehavior.Marker.RowHeaderColumn = format.TimesLocator.HeaderSeriesPosition;
                markupBehavior.Marker.ColumnHeaderRow = format.ValuesLocator.HeaderSeriesPosition;

                markupBehavior.Marker.SkipColumns = format.Excludes.ToArray();
                markupBehavior.Marker.SkipRows = null;
            }

            markupBehavior.Apply();

            markupBehavior.Detach();
        }

        public DataTable GetResult( IFigureDescriptor format )
        {
            Contract.RequiresNotNull( format, "format" );
            Contract.Invariant( Document != null, "Document not yet loaded" );

            var parser = DocumentProcessorsFactory.CreateParser( Document, format );
            return parser.ExtractTable();
        }
    }
}
