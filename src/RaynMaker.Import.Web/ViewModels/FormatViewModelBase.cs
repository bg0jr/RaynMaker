using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Documents.WinForms;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Web.ViewModels
{
    class FormatViewModelBase : BindableBase
    {
        private Type mySelectedDatum;
        private bool myInMillions;

        protected FormatViewModelBase( IFigureDescriptor format )
        {
            Contract.RequiresNotNull( format, "format" );

            Format = format;

            Datums = Dynamics.AllDatums
                .OrderBy( d => d.Name )
                .ToList();

            MarkupDocument = new MarkupDocument();
            MarkupDocument.SelectionChanged += OnSelectionChanged;
        }

        public IFigureDescriptor Format { get; private set; }

        // TODO: we do not support add, remove and edit of datums as they are currently fixed by entities model.
        // TODO: "Standing" datums also exists
        public IEnumerable<Type> Datums { get; private set; }

        public Type SelectedDatum
        {
            get { return mySelectedDatum; }
            set
            {
                if( SetProperty( ref mySelectedDatum, value ) )
                {
                    Format.Figure = mySelectedDatum.Name;
                }
            }
        }

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
                    var htmlDocument = ( IHtmlDocument )value;
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

        public bool InMillions
        {
            get { return myInMillions; }
            set
            {
                if( SetProperty( ref myInMillions, value ) )
                {
                    Format.InMillions = myInMillions;
                }
            }
        }
    }
}
