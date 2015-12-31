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
        private SeriesOrientation mySelectedDimension;
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
            SkipValues = string.Join( ",", format.Excludes );
            TimeFormat = Format.TimeFormat ?? new FormatColumn( "time" );
            ValueFormat = Format.ValueFormat ?? new FormatColumn( "value" );
            InMillions = Format.InMillions;

            ColumnHeaderRow = ( format.Orientation == SeriesOrientation.Row ? Format.TimesLocator.SeriesToScan : Format.ValuesLocator.SeriesToScan ).ToString();
            RowHeaderColumn = ( format.Orientation == SeriesOrientation.Row ? Format.ValuesLocator.SeriesToScan : Format.TimesLocator.SeriesToScan ).ToString();

            // needs to be AFTER RowHeaderColumn and ColumnHeaderRow
            SelectedDimension = Format.Orientation;
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

        public SeriesOrientation SelectedDimension
        {
            get { return mySelectedDimension; }
            set
            {
                if( SetProperty( ref mySelectedDimension, value ) )
                {
                    MarkupDocument.Dimension = mySelectedDimension;
                }
            }
        }

        //public string SeriesName
        //{
        //    get { return mySeriesName; }
        //    set
        //    {
        //        if( SetProperty( ref mySeriesName, value ) )
        //        {
        //            UpdateAnchor();

        //            Format.SeriesName = mySeriesName;

        //            MarkupDocument.SeriesName = mySeriesName;
        //        }
        //    }
        //}

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
                    Format.Excludes = GetIntArray( mySkipValues );
                    MarkupDocument.SkipRows = Format.Excludes;
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
