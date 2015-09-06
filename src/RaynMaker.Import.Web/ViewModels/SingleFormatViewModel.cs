using System;
using System.Linq;
using System.Windows.Forms;
using Blade;
using Blade.Data;
using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Import.Html;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web.ViewModels
{
    class SingleFormatViewModel : BindableBase
    {
        private string myPath;
        private string myValue;
        private CellDimension mySelectedDimension;
        private string mySeriesName;
        private bool myIsValid;
        private string myRowHeaderColumn;
        private string myColumnHeaderRow;
        private string mySkipRows;
        private string mySkipColumns;
        private string myTimeFormat;
        private string myValueFormat;
        private MarkupDocument myMarkupDocument;

        public SingleFormatViewModel( PathSeriesFormat format )
        {
            Contract.RequiresNotNull( format, "format" );

            Format = format;

            IsValid = true;

            myMarkupDocument = new MarkupDocument();
            myMarkupDocument.ValidationChanged += SeriesName_ValidationChanged;

            Value = "";

            Path = Format.Path;
            SkipColumns = string.Join( ",", format.SkipColumns );
            SkipRows = string.Join( ",", format.SkipRows );
            RowHeaderColumn = ( format.Expand == CellDimension.Row ? Format.TimeAxisPosition : Format.SeriesNamePosition ).ToString();
            ColumnHeaderRow = ( format.Expand == CellDimension.Row ? Format.SeriesNamePosition : Format.TimeAxisPosition ).ToString();
            SeriesName = format.SeriesNamePosition.ToString();
            TimeFormat = Format.TimeAxisFormat.Format;
            ValueFormat = Format.ValueFormat.Format;
        }

        public PathSeriesFormat Format { get; private set; }

        private void SeriesName_ValidationChanged( bool isValid )
        {
            IsValid = isValid;
        }

        public HtmlDocument Document
        {
            get { return myMarkupDocument.Document; }
            set
            {
                if( myMarkupDocument.Document == value )
                {
                    return;
                }

                if( myMarkupDocument.Document != null )
                {
                    myMarkupDocument.Document.Click -= HtmlDocument_Click;
                }

                myMarkupDocument.Document = value;

                myMarkupDocument.Document.Click += HtmlDocument_Click;
            }
        }

        private void HtmlDocument_Click( object sender, System.Windows.Forms.HtmlElementEventArgs e )
        {
            Path = myMarkupDocument.SelectedElement.GetPath().ToString();
            Value = myMarkupDocument.SelectedElement.InnerText;
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
                    UpdateHeaders();

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
                    UpdateHeaders();
                    MarkHeader( myRowHeaderColumn, x => myMarkupDocument.RowHeaderColumn = x );
                }
            }
        }

        private void UpdateHeaders()
        {
            if( Format.Expand == CellDimension.Row )
            {
                Format.TimeAxisPosition = int.Parse( ColumnHeaderRow );
                Format.SeriesNamePosition = int.Parse( RowHeaderColumn );
            }
            else if( Format.Expand == CellDimension.Column )
            {
                Format.TimeAxisPosition = int.Parse( RowHeaderColumn );
                Format.SeriesNamePosition = int.Parse( ColumnHeaderRow );
            }
        }

        private void MarkHeader( string text, Action<int> UpdateTemplate )
        {
            string str = text.TrimOrNull();
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
                    UpdateHeaders();
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
                    .Where( t => !t.IsNullOrTrimmedEmpty() )
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

        public string TimeFormat
        {
            get { return myTimeFormat; }
            set
            {
                if( SetProperty( ref myTimeFormat, value ) )
                {
                    Format.TimeAxisFormat = new FormatColumn( "time", typeof( int ), myTimeFormat );
                }
            }
        }

        public string ValueFormat
        {
            get { return myValueFormat; }
            set
            {
                if( SetProperty( ref myValueFormat, value ) )
                {
                    Format.ValueFormat = new FormatColumn( "value", typeof( double ), myValueFormat );
                }
            }
        }

        public void Apply()
        {
            myMarkupDocument.Apply();
        }
    }
}
