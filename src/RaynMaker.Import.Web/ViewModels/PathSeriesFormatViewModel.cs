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
        private string mySkipValues;

        public PathSeriesFormatViewModel( PathSeriesDescriptor format )
            : base( format )
        {
            Format = format;

            MarkupDocument.ValidationChanged += SeriesName_ValidationChanged;

            IsValid = true;

            Value = "";

            // first set properties without side-effects to others
            SelectedDatum = Datums.FirstOrDefault( d => d.Name == Format.Datum );
            Path = Format.Path;
            SkipValues = string.Join( ",", format.SkipValues );
            SeriesName = format.SeriesName;
            TimeFormat = Format.TimeFormat ?? new FormatColumn( "time" );
            ValueFormat = Format.ValueFormat ?? new FormatColumn( "value" );
            InMillions = Format.InMillions;

            if( format.Anchor != null )
            {
                ColumnHeaderRow = ( format.Anchor.Expand == CellDimension.Row ? Format.TimeAxisPosition : Format.Anchor.SeriesNamePosition ).ToString();
                RowHeaderColumn = ( format.Anchor.Expand == CellDimension.Row ? Format.Anchor.SeriesNamePosition : Format.TimeAxisPosition ).ToString();

                // needs to be AFTER RowHeaderColumn and ColumnHeaderRow
                SelectedDimension = Format.Anchor.Expand;
            }
            else
            {
                SelectedDimension = CellDimension.None;
            }
        }

        public new PathSeriesDescriptor Format { get; private set; }

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
                    if( mySelectedDimension == CellDimension.Row )
                    {
                        Format.TimeAxisPosition = ColumnHeaderRow != null ? int.Parse( ColumnHeaderRow ) : -1;
                    }
                    else if( mySelectedDimension == CellDimension.Column )
                    {
                        Format.TimeAxisPosition = RowHeaderColumn != null ? int.Parse( RowHeaderColumn ) : -1;
                    }

                    UpdateAnchor();

                    MarkupDocument.Dimension = mySelectedDimension;
                }
            }
        }

        private void UpdateAnchor()
        {
            if( mySelectedDimension == CellDimension.None )
            {
                Format.Anchor = null;
            }
            else if( mySelectedDimension == CellDimension.Row )
            {
                Format.Anchor = TableFragmentDescriptor.ForRow( new StringContainsLocator( int.Parse( myRowHeaderColumn ), SeriesName ) );
            }
            else if( mySelectedDimension == CellDimension.Column )
            {
                Format.Anchor = TableFragmentDescriptor.ForColumn( new StringContainsLocator( int.Parse( myColumnHeaderRow ), SeriesName ) );
            }
        }

        public string SeriesName
        {
            get { return mySeriesName; }
            set
            {
                if( SetProperty( ref mySeriesName, value ) )
                {
                    UpdateAnchor();

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
                    if( Format.Anchor != null && Format.Anchor.Expand == CellDimension.Row )
                    {
                        UpdateAnchor();
                    }
                    else if( Format.Anchor != null && Format.Anchor.Expand == CellDimension.Column )
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
                    if( Format.Anchor != null && Format.Anchor.Expand == CellDimension.Row )
                    {
                        Format.TimeAxisPosition = ColumnHeaderRow != null ? int.Parse( ColumnHeaderRow ) : -1;
                    }
                    else if( Format.Anchor != null && Format.Anchor.Expand == CellDimension.Column )
                    {
                        UpdateAnchor();
                    }

                    MarkHeader( myColumnHeaderRow, x => MarkupDocument.ColumnHeaderRow = x );
                }
            }
        }

        public string SkipValues
        {
            get { return mySkipValues; }
            set
            {
                if( SetProperty( ref mySkipValues, value ) )
                {
                    Format.SkipValues = GetIntArray( mySkipValues );
                    MarkupDocument.SkipRows = Format.SkipValues;
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

        public FormatColumn TimeFormat { get; private set; }

        public FormatColumn ValueFormat { get; private set; }
    }
}
