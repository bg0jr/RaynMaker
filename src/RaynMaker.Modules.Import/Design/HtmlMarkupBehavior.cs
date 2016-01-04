using System;
using System.Drawing;
using System.Windows.Forms;
using Plainion;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Design
{
    public class HtmlMarkupBehavior : IDisposable
    {
        private HtmlElementMarker myElementMarker;
        private HtmlTableMarker myTableMarker;

        private HtmlDocumentAdapter myDocument;
        // holds the element which has been marked by the user "click"
        // (before extensions has been applied)
        private HtmlElementAdapter mySelectedElement;

        private string myPath;

        public HtmlMarkupBehavior()
        {
            myElementMarker = new HtmlElementMarker( Color.Yellow );
            myTableMarker = new HtmlTableMarker( Color.Yellow, Color.SteelBlue );

            Reset();
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

                if( value == null )
                {
                    return;
                }

                myDocument = new HtmlDocumentAdapter( value );
                myDocument.Document.Click += HtmlDocument_Click;

                // Internally adjusts SelectedElement
                PathToSelectedElement = PathToSelectedElement;
            }
        }

        private void HtmlDocument_Click( object sender, HtmlElementEventArgs e )
        {
            var element = myDocument.Document.GetElementFromPoint( e.ClientMousePosition );

            SelectedElement = myDocument.Create( element );
        }

        public Action SelectionChanged = null;

        public HtmlElementAdapter SelectedElement
        {
            get { return mySelectedElement; }
            set
            {
                Contract.Invariant( myDocument != null, "Document not attached" );

                UnmarkAll();

                mySelectedElement = value;

                if( mySelectedElement != null )
                {
                    Apply();

                    mySelectedElement.Element.ScrollIntoView( false );
                }

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
                myPath = value;

                UpdateSelectedElement();
            }
        }

        private void UpdateSelectedElement()
        {
            if( myDocument != null && myPath != null )
            {
                var path = HtmlPath.TryParse( myPath );
                if( path == null )
                {
                    // TODO: signal error to UI
                    return;
                }
                SelectedElement = ( HtmlElementAdapter )myDocument.GetElementByPath( path );
            }
            else
            {
                SelectedElement = null;
            }
        }

        public void Dispose()
        {
            if( myDocument != null )
            {
                myDocument.Document.Click -= HtmlDocument_Click;
            }
        }

        public void Reset()
        {
            myElementMarker.Unmark();
            myTableMarker.Reset();

            mySelectedElement = null;
        }

        public void Apply()
        {
            if( mySelectedElement == null )
            {
                UpdateSelectedElement();
            }

            if( mySelectedElement == null || mySelectedElement.TagName.Equals( "INPUT", StringComparison.OrdinalIgnoreCase ) )
            {
                return;
            }

            // unmark all first
            UnmarkAll();

            if( mySelectedElement.FindEmbeddingTable() != null )
            {
                myTableMarker.Mark( mySelectedElement );
            }
            else
            {
                myElementMarker.Mark( mySelectedElement );
            }
        }

        public void UnmarkAll()
        {
            myElementMarker.Unmark();
            myTableMarker.Unmark();
        }
    }
}