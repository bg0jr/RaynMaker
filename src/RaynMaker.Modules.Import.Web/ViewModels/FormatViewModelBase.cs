using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Documents.WinForms;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class FormatViewModelBase<TMarker> : BindableBase, IDescriptorViewModel where TMarker : IHtmlMarker
    {
        private Type mySelectedDatum;
        private bool myInMillions;

        protected FormatViewModelBase( IFigureDescriptor descriptor, TMarker marker )
        {
            Contract.RequiresNotNull( descriptor, "descriptor" );

            Format = descriptor;

            Datums = Dynamics.AllDatums
                .OrderBy( d => d.Name )
                .ToList();

            MarkupBehavior = new HtmlMarkupBehavior<TMarker>( marker );
            MarkupBehavior.SelectionChanged += OnSelectionChanged;
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

        protected HtmlMarkupBehavior<TMarker> MarkupBehavior { get; private set; }

        private void OnSelectionChanged( object sender, EventArgs e )
        {
            OnSelectionChanged();
        }

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
                    var adapter = ( HtmlDocumentAdapter )value;
                    doc = adapter.Document;
                }

                MarkupBehavior.AttachTo( doc );

                if( doc == null )
                {
                    return;
                }

                OnSelectionChanged();
            }
        }

        public void Apply()
        {
            MarkupBehavior.Apply();
        }

        public void Unmark()
        {
            MarkupBehavior.Marker.Unmark();
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
