using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Import.Documents;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Parsers.Html.WinForms;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    class SingleFormatViewModel : BindableBase
    {
        private Type mySelectedDatum;
        private string myPath;
        private string myValue;
        private CellDimension mySelectedDimension;
        private string mySeriesName;
        private bool myIsValid;
        private string myRowHeaderColumn;
        private string myColumnHeaderRow;
        private string mySkipRows;
        private string mySkipColumns;
        private bool myInMillions;
        private MarkupDocument myMarkupDocument;

        public SingleFormatViewModel( PathSeriesFormat format )
        {
            Contract.RequiresNotNull( format, "format" );

            Format = format;

            IsValid = true;

            myMarkupDocument = new MarkupDocument();
            myMarkupDocument.ValidationChanged += SeriesName_ValidationChanged;
            myMarkupDocument.SelectionChanged += OnSelectionChanged;

            Value = "";

            Datums = Dynamics.AllDatums
                .OrderBy( d => d.Name )
                .ToList();

            // first set properties without side-effects to others
            SelectedDatum = Datums.FirstOrDefault( d => d.Name == Format.Datum );
            Path = Format.Path;
            SkipColumns = string.Join( ",", format.SkipColumns );
            SkipRows = string.Join( ",", format.SkipRows );
            SeriesName = format.SeriesName;
            TimeFormat = Format.TimeAxisFormat ?? new FormatColumn( "time" );
            ValueFormat = Format.ValueFormat ?? new FormatColumn( "value" );

            ColumnHeaderRow = ( format.Expand == CellDimension.Row ? Format.TimeAxisPosition : Format.SeriesNamePosition ).ToString();
            RowHeaderColumn = ( format.Expand == CellDimension.Row ? Format.SeriesNamePosition : Format.TimeAxisPosition ).ToString();

            // needs to be AFTER RowHeaderColumn and ColumnHeaderRow
            SelectedDimension = Format.Expand;
        }

        public PathSeriesFormat Format { get; private set; }

        private void SeriesName_ValidationChanged( bool isValid )
        {
            IsValid = isValid;
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

                myMarkupDocument.Document = doc;

                if( myMarkupDocument.Document == null )
                {
                    return;
                }

                OnSelectionChanged();
            }
        }

        private void OnSelectionChanged()
        {
            if( myMarkupDocument.SelectedElement != null )
            {
                Path = myMarkupDocument.SelectedElement.GetPath().ToString();
                Value = myMarkupDocument.SelectedElement.InnerText;
            }
        }

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
                    Format.Datum = mySelectedDatum.Name;
                }
            }
        }

        public string Path
        {
            get { return myPath; }
            set
            {
                if( SetProperty( ref myPath, value ) )
                {
                    Format.Path = myPath;

                    if( !string.IsNullOrWhiteSpace( myPath ) )
                    {
                        myMarkupDocument.Anchor = myPath;

                        if( myMarkupDocument.SelectedElement != null )
                        {
                            Value = myMarkupDocument.SelectedElement.InnerText;
                        }
                    }
                }
            }
        }

        public string Value
        {
            get { return myValue; }
            set { SetProperty( ref myValue, value ); }
        }

        public CellDimension SelectedDimension
        {
            get { return mySelectedDimension; }
            set
            {
                if( SetProperty( ref mySelectedDimension, value ) )
                {
                    Format.Expand = mySelectedDimension;

                    if( Format.Expand == CellDimension.Row )
                    {
                        Format.TimeAxisPosition = ColumnHeaderRow != null ? int.Parse( ColumnHeaderRow ) : -1;
                        Format.SeriesNamePosition = RowHeaderColumn != null ? int.Parse( RowHeaderColumn ) : -1;
                    }
                    else if( Format.Expand == CellDimension.Column )
                    {
                        Format.TimeAxisPosition = RowHeaderColumn != null ? int.Parse( RowHeaderColumn ) : -1;
                        Format.SeriesNamePosition = ColumnHeaderRow != null ? int.Parse( ColumnHeaderRow ) : -1;
                    }

                    myMarkupDocument.Dimension = mySelectedDimension;
                }
            }
        }

        public string SeriesName
        {
            get { return mySeriesName; }
            set
            {
                if( SetProperty( ref mySeriesName, value ) )
                {
                    Format.SeriesName = mySeriesName;

                    myMarkupDocument.SeriesName = mySeriesName;
                }
            }
        }

        public bool IsValid
        {
            get { return myIsValid; }
            set { SetProperty( ref myIsValid, value ); }
        }

        public string RowHeaderColumn
        {
            get { return myRowHeaderColumn; }
            set
            {
                if( SetProperty( ref myRowHeaderColumn, value ) )
                {
                    // only update EXACTLY the corresponding model
                    if( Format.Expand == CellDimension.Row )
                    {
                        Format.SeriesNamePosition = RowHeaderColumn != null ? int.Parse( RowHeaderColumn ) : -1;
                    }
                    else if( Format.Expand == CellDimension.Column )
                    {
                        Format.TimeAxisPosition = RowHeaderColumn != null ? int.Parse( RowHeaderColumn ) : -1;
                    }

                    MarkHeader( myRowHeaderColumn, x => myMarkupDocument.RowHeaderColumn = x );
                }
            }
        }

        private void MarkHeader( string text, Action<int> UpdateTemplate )
        {
            string str = text != null ? text.Trim() : null;
            if( string.IsNullOrEmpty( str ) )
            {
                UpdateTemplate( -1 );
                return;
            }

            try
            {
                UpdateTemplate( Convert.ToInt32( str ) );
            }
            catch
            {
                //errorProvider1.SetError( config, "Must be: <number> [, <number> ]*" );
            }
        }

        public string ColumnHeaderRow
        {
            get { return myColumnHeaderRow; }
            set
            {
                if( SetProperty( ref myColumnHeaderRow, value ) )
                {
                    // only update EXACTLY the corresponding model
                    if( Format.Expand == CellDimension.Row )
                    {
                        Format.TimeAxisPosition = ColumnHeaderRow != null ? int.Parse( ColumnHeaderRow ) : -1;
                    }
                    else if( Format.Expand == CellDimension.Column )
                    {
                        Format.SeriesNamePosition = ColumnHeaderRow != null ? int.Parse( ColumnHeaderRow ) : -1;
                    }

                    MarkHeader( myColumnHeaderRow, x => myMarkupDocument.ColumnHeaderRow = x );
                }
            }
        }

        public string SkipRows
        {
            get { return mySkipRows; }
            set
            {
                if( SetProperty( ref mySkipRows, value ) )
                {
                    Format.SkipRows = GetIntArray( mySkipRows );
                    myMarkupDocument.SkipRows = Format.SkipRows;
                }
            }
        }

        private int[] GetIntArray( string value )
        {
            if( string.IsNullOrWhiteSpace( value ) )
            {
                return null;
            }

            try
            {
                return value.Split( ',' )
                    .Where( t => !string.IsNullOrWhiteSpace( t ) )
                    .Select( t => Convert.ToInt32( t ) )
                    .ToArray();
            }
            catch
            {
                //errorProvider1.SetError( config, "Must be: <number> [, <number> ]*" );
            }

            return null;
        }

        public string SkipColumns
        {
            get { return mySkipColumns; }
            set
            {
                if( SetProperty( ref mySkipColumns, value ) )
                {
                    Format.SkipColumns = GetIntArray( mySkipColumns );
                    myMarkupDocument.SkipColumns = Format.SkipColumns;
                }
            }
        }

        public FormatColumn TimeFormat { get; private set; }

        public FormatColumn ValueFormat { get; private set; }

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

        public void Apply()
        {
            myMarkupDocument.Apply();
        }

        internal void UnMark()
        {
            myMarkupDocument.UnmarkAll();
        }
    }
}
