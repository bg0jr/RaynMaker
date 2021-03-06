﻿using System;
using System.Windows.Forms;
using Plainion;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Parsers.Html;

namespace RaynMaker.Modules.Import.Design
{
    /// <summary>
    /// Applies highlighting of HTML elements to an HtmlDocument. 
    /// The hightligted HtmlElement can be defined by setting <see cref="SelectedElement"/>, <see cref="PathToSelectedElement"/> or
    /// by clicking into the HtmlDocument.
    /// This behavior implements a "single selection" behavior.
    /// </summary>
    public class HtmlMarkupBehavior<T> : IHtmlMarkupBehavior<T> where T : IHtmlMarker
    {
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
            if( Document != null )
            {
                Document.Document.Click -= HtmlDocument_Click;
            }
        }

        public HtmlDocumentAdapter Document { get; private set; }

        public void AttachTo( HtmlDocument document )
        {
            Contract.RequiresNotNull( document, "document" );

            AttachTo( new HtmlDocumentAdapter( document ) );
        }

        public void AttachTo( IHtmlDocument document )
        {
            Contract.RequiresNotNull( document, "document" );

            var documentAdapter = document as HtmlDocumentAdapter;

            Contract.Requires( documentAdapter != null, "Only documents of type {0} supported", typeof( HtmlDocumentAdapter ).FullName );

            // WebBrowser reused HtmlDocument instances
            // -> we cannot ignore assignment of same HtmlDocument here

            var behaviorHashCode = documentAdapter.Document.Body.GetAttribute( "RaynMakerHtmlMarkupBehavior" );
            Contract.Invariant( string.IsNullOrEmpty( behaviorHashCode ) || behaviorHashCode == GetHashCode().ToString(),
                "A HtmlMarkupBehavior already attached to the given HtmlDocument. Only one attached HtmlMarkupBehavior per HtmlDocument supported" );

            Detach();

            Document = documentAdapter;
            Document.Document.Click += HtmlDocument_Click;

            Document.Document.Body.SetAttribute( "RaynMakerHtmlMarkupBehavior", GetHashCode().ToString() );

            // In Detach() we resetted everything so nothing to apply here
            // Apply();
        }

        public void Detach()
        {
            if( Document == null )
            {
                return;
            }

            Document.Document.Click -= HtmlDocument_Click;

            mySelectedElement = null;
            myPath = null;

            // We have to unmark all because anyway with new document the HtmlElements inside the Marker are invalid.
            // We do not call Reset() in order to keep the settings in the Marker (e.g. HtmlTableMarker).
            Marker.Unmark();

            Document.Document.Body.SetAttribute( "RaynMakerHtmlMarkupBehavior", null );

            Document = null;
        }

        private void HtmlDocument_Click( object sender, HtmlElementEventArgs e )
        {
            var element = HtmlMarkupAutomationProvider.GetClickedElement( Document.Document );
            if ( element == null )
            {
                element = Document.Document.GetElementFromPoint( e.ClientMousePosition );
            }

            SelectedElement = Document.Create( element );
        }

        public event EventHandler SelectionChanged;

        public IHtmlElement SelectedElement
        {
            get { return mySelectedElement; }
            set
            {
                Contract.Invariant( Document != null, "Document not attached" );

                if( mySelectedElement == value )
                {
                    return;
                }

                var elementAdapter = value as HtmlElementAdapter;

                Contract.Requires( elementAdapter != null, "Only elements of type {0} supported", typeof( HtmlElementAdapter ).FullName );

                mySelectedElement = elementAdapter;
                // sync with SelectedElement in Apply()
                myPath = null;

                Apply();

                if( SelectionChanged != null )
                {
                    SelectionChanged( this, EventArgs.Empty );
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

        private void Apply()
        {
            Marker.Unmark();

            if( Document == null )
            {
                return;
            }

            if( mySelectedElement == null && myPath != null )
            {
                var path = HtmlPath.TryParse( myPath );
                if( path != null )
                {
                    // Trigger SelectionChanged event
                    SelectedElement = ( HtmlElementAdapter )Document.GetElementByPath( path );
                }
            }
            else if( mySelectedElement != null && myPath == null )
            {
                // do NOT set property - it will reset SelectedElement again (StackOverflowException)
                myPath = mySelectedElement.GetPath().ToString();
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