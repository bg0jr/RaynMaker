using System;
using System.Windows.Forms;
using Plainion;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Design
{
    public class HtmlMarkupBehavior<T> : IDisposable where T : IHtmlMarker
    {
        private HtmlDocumentAdapter myDocument;
        // holds the element which has been marked by the user "click"
        // (before extensions has been applied)
        private HtmlElementAdapter mySelectedElement;

        private string myPath;

        public HtmlMarkupBehavior( T marker )
        {
            Marker = marker;
        }

        public T Marker { get; private set; }

        public void Dispose()
        {
            if( myDocument != null )
            {
                myDocument.Document.Click -= HtmlDocument_Click;
            }
        }

        public HtmlDocument Document
        {
            get { return myDocument != null ? myDocument.Document : null; }
            set
            {
                if( myDocument != null )
                {
                    myDocument.Document.Click -= HtmlDocument_Click;
                }

                mySelectedElement = null;
                myPath = null;
                // We have to unmark all because anyway with new document the HtmlElements inside the Marker are invalid.
                // We do not call Reset() in order to keep the settings in the Marker (e.g. HtmlTableMarker).
                Marker.Unmark();

                if( value == null )
                {
                    return;
                }

                myDocument = new HtmlDocumentAdapter( value );
                myDocument.Document.Click += HtmlDocument_Click;

                Apply();
            }
        }

        private void HtmlDocument_Click( object sender, HtmlElementEventArgs e )
        {
            var element = myDocument.Document.GetElementFromPoint( e.ClientMousePosition );

            SelectedElement = myDocument.Create( element );
        }

        public Action SelectionChanged { get; set; }

        public HtmlElementAdapter SelectedElement
        {
            get { return mySelectedElement; }
            set
            {
                Contract.Invariant( myDocument != null, "Document not attached" );

                if( mySelectedElement == value )
                {
                    return;
                }

                mySelectedElement = value;
                // sync with SelectedElement in Apply()
                myPath = null;

                Apply();

                if( SelectionChanged != null )
                {
                    SelectionChanged();
                }
            }
        }

        public string PathToSelectedElement
        {
            get { return myPath; }
            set
            {
                if( myPath == value )
                {
                    return;
                }

                myPath = value;
                // sync with PathToSelectedElement in Apply()
                mySelectedElement = null;

                Apply();
            }
        }

        public void Apply()
        {
            Marker.Unmark();

            if( myDocument == null )
            {
                return;
            }

            if( mySelectedElement == null && myPath != null )
            {
                var path = HtmlPath.TryParse( myPath );
                if( path != null )
                {
                    // Trigger SelectionChanged event
                    SelectedElement = ( HtmlElementAdapter )myDocument.GetElementByPath( path );
                }
            }
            else if( mySelectedElement != null && myPath == null )
            {
                PathToSelectedElement = mySelectedElement.GetPath().ToString();
            }

            if( mySelectedElement == null || mySelectedElement.TagName.Equals( "INPUT", StringComparison.OrdinalIgnoreCase ) )
            {
                return;
            }

            Marker.Mark( mySelectedElement );

            mySelectedElement.Element.ScrollIntoView( false );
        }
    }
}