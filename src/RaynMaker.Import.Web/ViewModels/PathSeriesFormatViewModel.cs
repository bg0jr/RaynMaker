using System;
using System.Linq;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Web.ViewModels
{
    class PathSeriesFormatViewModel : FormatViewModelBase
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

        public PathSeriesFormatViewModel( PathSeriesExtractionDescriptor format )
            :base(format)
        {
            Format = format;
            
            MarkupDocument.ValidationChanged += SeriesName_ValidationChanged;
            
            IsValid = true;

            Value = "";

            // first set properties without side-effects to others
            SelectedDatum = Datums.FirstOrDefault( d => d.Name == Format.Datum );
            Path = Format.Path;
            SkipColumns = string.Join( ",", format.SkipColumns );
            SkipRows = string.Join( ",", format.SkipRows );
            SeriesName = format.SeriesName;
            TimeFormat = Format.TimeAxisFormat ?? new FormatColumn( "time" );
            ValueFormat = Format.ValueFormat ?? new FormatColumn( "value" );
            InMillions = Format.InMillions;

            ColumnHeaderRow = ( format.Expand == CellDimension.Row ? Format.TimeAxisPosition : Format.SeriesNamePosition ).ToString();
            RowHeaderColumn = ( format.Expand == CellDimension.Row ? Format.SeriesNamePosition : Format.TimeAxisPosition ).ToString();

            // needs to be AFTER RowHeaderColumn and ColumnHeaderRow
            SelectedDimension = Format.Expand;
        }

        public new PathSeriesExtractionDescriptor Format { get; private set; }
        
        private void SeriesName_ValidationChanged( bool isValid )
        {
            IsValid = isValid;
        }

        protected override void OnSelectionChanged()
        {
            if( MarkupDocument.SelectedElement != null )
            {
                Path = MarkupDocument.SelectedElement.GetPath().ToString();
                Value = MarkupDocument.SelectedElement.InnerText;
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
                        MarkupDocument.Anchor = myPath;

                        if( MarkupDocument.SelectedElement != null )
                        {
                            Value = MarkupDocument.SelectedElement.InnerText;
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

                    MarkupDocument.Dimension = mySelectedDimension;
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

                    MarkupDocument.SeriesName = mySeriesName;
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

                    MarkHeader( myRowHeaderColumn, x => MarkupDocument.RowHeaderColumn = x );
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

                    MarkHeader( myColumnHeaderRow, x => MarkupDocument.ColumnHeaderRow = x );
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
                    MarkupDocument.SkipRows = Format.SkipRows;
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
                    MarkupDocument.SkipColumns = Format.SkipColumns;
                }
            }
        }

        public FormatColumn TimeFormat { get; private set; }

        public FormatColumn ValueFormat { get; private set; }
    }
}
