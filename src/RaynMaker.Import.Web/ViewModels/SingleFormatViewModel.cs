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
        private MarkupDocument myMarkupDocument;

        public SingleFormatViewModel( PathSeriesFormat format )
        {
            Contract.RequiresNotNull( format, "format" );

            Format = format;
            
            IsValid = true;

            myMarkupDocument = new MarkupDocument();
            myMarkupDocument.ValidationChanged += SeriesName_ValidationChanged;

            Path = "";
            Value = "";

            SkipColumns = null;
            SkipRows = null;
            RowHeaderColumn = null;
            ColumnHeaderRow = null;
            SeriesName = null;
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
                    MarkHeader( myRowHeaderColumn, x => myMarkupDocument.RowHeader = x );
                }
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
                    MarkHeader( myColumnHeaderRow, x => myMarkupDocument.ColumnHeader = x );
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
                    SkipElements( mySkipRows, x => myMarkupDocument.SkipRows = x );
                }
            }
        }

        private void SkipElements( string text, Action<int[]> UpdateTemplate )
        {
            if( text.IsNullOrTrimmedEmpty() )
            {
                UpdateTemplate( null );
                return;
            }

            string[] tokens = text.Split( ',' );

            try
            {
                var positions = from t in tokens
                                where !t.IsNullOrTrimmedEmpty()
                                select Convert.ToInt32( t );

                UpdateTemplate( positions.ToArray() );
            }
            catch
            {
                //errorProvider1.SetError( config, "Must be: <number> [, <number> ]*" );
            }
        }

        public string SkipColumns
        {
            get { return mySkipColumns; }
            set
            {
                if( SetProperty( ref mySkipColumns, value ) )
                {
                    SkipElements( mySkipColumns, x => myMarkupDocument.SkipColumns = x );
                }
            }
        }

        public void Apply()
        {
            myMarkupDocument.Apply();
        }
    }
}
