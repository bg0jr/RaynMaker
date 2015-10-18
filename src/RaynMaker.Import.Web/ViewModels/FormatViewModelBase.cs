using System.Windows.Forms;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html.WinForms;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    class FormatViewModelBase : BindableBase
    {
        protected FormatViewModelBase( IFormat format )
        {
            Contract.RequiresNotNull( format, "format" );

            Format = format;

            MarkupDocument = new MarkupDocument();
            MarkupDocument.SelectionChanged += OnSelectionChanged;
        }

        public IFormat Format { get; private set; }

        protected MarkupDocument MarkupDocument { get; private set; }

        protected virtual void OnSelectionChanged()
        {
        }

        public IDocument Document
        {
            set
            {
                // always force update because the document reference does NOT change!
                //if( myMarkupDocument.Document == value )
                //{
                //    return;
                //}

                HtmlDocument doc = null;
                if( value != null )
                {
                    var htmlDocument = ( ( HtmlDocumentHandle )value ).Content;
                    var adapter = ( HtmlDocumentAdapter )htmlDocument;
                    doc = adapter.Document;
                }

                MarkupDocument.Document = doc;

                if( MarkupDocument.Document == null )
                {
                    return;
                }

                OnSelectionChanged();
            }
        }

        public void Apply()
        {
            MarkupDocument.Apply();
        }

        public void UnMark()
        {
            MarkupDocument.UnmarkAll();
        }
    }
}
