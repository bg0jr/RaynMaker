using System;
using System.ComponentModel;
using System.Linq;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using Plainion;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class PathSeriesFormatViewModel : FormatViewModelBase<PathSeriesDescriptor, HtmlTableMarker>
    {
        private string myPath;
        private string myValue;
        private SeriesOrientation mySelectedDimension;
        private bool myIsValid;
        private int myValuesPosition;
        private string myValuesPattern;
        private int myTimesPosition;
        private string mySkipValues;

        public PathSeriesFormatViewModel( PathSeriesDescriptor descriptor )
            : this( descriptor, new HtmlMarkupBehavior<HtmlTableMarker>( new HtmlTableMarker() ) )
        {
        }

        /// <summary>
        /// UT only!
        /// </summary>
        internal PathSeriesFormatViewModel( PathSeriesDescriptor descriptor, IHtmlMarkupBehavior<HtmlTableMarker> markupBehavior )
            : base( descriptor, markupBehavior )
        {
            Value = "";

            SelectedDatum = Datums.FirstOrDefault( d => d.Name == descriptor.Figure );
            Path = descriptor.Path;

            if ( descriptor.ValueFormat == null )
            {
                descriptor.ValueFormat = new FormatColumn( "value", typeof( double ) );
            }
            ValueFormat = descriptor.ValueFormat;
            PropertyChangedEventManager.AddHandler( ValueFormat, OnValueFormatChanged, string.Empty );

            if ( descriptor.TimeFormat == null )
            {
                descriptor.TimeFormat = new FormatColumn( "year", typeof( int ) );
            }
            ValueFormat = descriptor.TimeFormat;

            IsValid = true;
            if ( descriptor.ValuesLocator != null )
            {
                ValuesPattern = ( (StringContainsLocator)Format.ValuesLocator ).Pattern;
                ValuesPosition = Format.ValuesLocator.HeaderSeriesPosition;
            }
            else
            {
                descriptor.ValuesLocator = new StringContainsLocator();
                ValuesPosition = -1;
                ValuesPattern = null;
            }

            if ( descriptor.TimesLocator != null )
            {
                TimesPosition = ( (AbsolutePositionLocator)Format.TimesLocator ).SeriesPosition;
            }
            else
            {
                Format.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0 };
                TimesPosition = -1;
            }

            SelectedOrientation = Format.Orientation;

            SkipValues = string.Join( ",", descriptor.Excludes );

            ValidateValuesLocator();
            ValidateTimesLocator();
        }

        private void OnValueFormatChanged( object sender, PropertyChangedEventArgs e )
        {
            TryUpdateValue();
        }

        protected override void OnDocumentChanged()
        {
            var cell = GetHtmlElementFromDescription();
            MarkupBehavior.PathToSelectedElement = cell != null ? cell.GetPath().ToString() : null;
        }

        private IHtmlElement GetHtmlElementFromDescription()
        {
            if ( string.IsNullOrEmpty( Path ) )
            {
                return null;
            }

            var table = HtmlTable.FindByPath( (IHtmlDocument)Document, HtmlPath.Parse( Path ) );
            if ( table == null )
            {
                return null;
            }

            if ( Format.Orientation == SeriesOrientation.Column )
            {
                int rowToScan = Format.ValuesLocator.HeaderSeriesPosition;
                if ( 0 > rowToScan || rowToScan >= table.Rows.Count )
                {
                    return null;
                }

                var colIdx = Format.ValuesLocator.FindIndex( table.GetRow( rowToScan ).Select( item => item.InnerText ) );
                if ( colIdx == -1 )
                {
                    return null;
                }

                int rowIdx = 0;
                if ( rowIdx == Format.ValuesLocator.HeaderSeriesPosition && rowIdx < table.Rows.Count )
                {
                    rowIdx++;
                }

                return table.GetCellAt( rowIdx, colIdx );
            }
            else if ( Format.Orientation == SeriesOrientation.Row )
            {
                var colToScan = Format.ValuesLocator.HeaderSeriesPosition;
                if ( 0 > colToScan || colToScan >= table.GetRow( 0 ).Count() )
                {
                    return null;
                }

                var rowIdx = Format.ValuesLocator.FindIndex( table.GetColumn( colToScan ).Select( item => item.InnerText ) );
                if ( rowIdx == -1 )
                {
                    return null;
                }

                int colIdx = 0;
                if ( colIdx == Format.ValuesLocator.HeaderSeriesPosition && colIdx < table.GetRow( rowIdx ).Count )
                {
                    colIdx++;
                }

                return table.GetCellAt( rowIdx, colIdx );
            }

            throw new NotSupportedException( "Unknown orientation: " + Format.Orientation );
        }

        protected override void OnSelectionChanged()
        {
            Path = MarkupBehavior.PathToSelectedElement;

            TryAutoDetectCellFromSelection( MarkupBehavior.SelectedElement );

            ValidateValuesLocator();
            ValidateTimesLocator();
        }

        /// <summary>
        /// Path should come from user click which directly points to the wanted cell.
        /// </summary>
        private void TryAutoDetectCellFromSelection( IHtmlElement selectedElement )
        {
            if ( !selectedElement.GetPath().PointsToTableCell )
            {
                return;
            }

            var table = MarkupBehavior.Marker.Table;
            if ( table == null )
            {
                return;
            }

            if ( ValuesPosition == -1 )
            {
                // just guess ...
                ValuesPosition = 0;
            }

            if ( TimesPosition == -1 )
            {
                // just guess ...
                TimesPosition = 0;
            }

            ValuesPattern = ( Format.Orientation == SeriesOrientation.Row
                ? table.GetCellAt( table.GetColumnIndex( selectedElement ), ValuesPosition )
                : table.GetCellAt( ValuesPosition, table.GetColumnIndex( selectedElement ) ) )
                .InnerText.Trim();
        }

        public string Path
        {
            get { return myPath; }
            set
            {
                if ( SetProperty( ref myPath, value ) )
                {
                    // Path must point to table NOT to cell in table
                    var path = value == null ? null : HtmlPath.Parse( value ).GetPathToTable();

                    // in case we do not point to table we set the original path
                    if ( SetProperty( ref myPath, path != null ? path.ToString() : value ) )
                    {
                        Format.Path = myPath;

                        // only overwrite if it was not set yet. Once the user has clicked we want
                        // to keep the user selection and NOT apply the reduced path (which is not supported by HtmlTableMarker)
                        if ( string.IsNullOrEmpty( MarkupBehavior.PathToSelectedElement ) )
                        {
                            MarkupBehavior.PathToSelectedElement = myPath;
                        }
                    }
                }
            }
        }

        public string Value
        {
            get { return myValue; }
            private set { SetProperty( ref myValue, value ); }
        }

        public SeriesOrientation SelectedOrientation
        {
            get { return mySelectedDimension; }
            set
            {
                if ( SetProperty( ref mySelectedDimension, value ) )
                {
                    if ( SelectedOrientation == SeriesOrientation.Row )
                    {
                        MarkupBehavior.Marker.ExpandColumn = false;
                        MarkupBehavior.Marker.ExpandRow = true;
                    }
                    else if ( SelectedOrientation == SeriesOrientation.Column )
                    {
                        MarkupBehavior.Marker.ExpandColumn = true;
                        MarkupBehavior.Marker.ExpandRow = false;
                    }
                }
            }
        }

        public int ValuesPosition
        {
            get { return myValuesPosition; }
            set
            {
                if ( SetProperty( ref myValuesPosition, value ) )
                {
                    ( (StringContainsLocator)Format.ValuesLocator ).HeaderSeriesPosition = myValuesPosition;
                    ValidateValuesLocator();

                    if ( Format.Orientation == SeriesOrientation.Column )
                    {
                        MarkupBehavior.Marker.ColumnHeaderRow = myValuesPosition;
                    }
                    else if ( Format.Orientation == SeriesOrientation.Row )
                    {
                        MarkupBehavior.Marker.RowHeaderColumn = myValuesPosition;
                    }
                }
            }
        }

        private void ValidateValuesLocator()
        {
            var table = MarkupBehavior.Marker.Table;

            if ( table != null && ValuesPattern != null )
            {
                if ( Format.Orientation == SeriesOrientation.Column )
                {
                    var header = table.GetCellAt( Format.ValuesLocator.HeaderSeriesPosition, table.GetRowIndex( MarkupBehavior.SelectedElement ) ).InnerText;
                    IsValid = header.Contains( ValuesPattern, StringComparison.OrdinalIgnoreCase );
                }
                else if ( Format.Orientation == SeriesOrientation.Row )
                {
                    var header = table.GetCellAt( table.GetRowIndex( MarkupBehavior.SelectedElement ), Format.ValuesLocator.HeaderSeriesPosition ).InnerText;
                    IsValid = header.Contains( ValuesPattern, StringComparison.OrdinalIgnoreCase );
                }

                TryUpdateValue();
            }
        }

        private void TryUpdateValue()
        {
            if ( !IsValid || string.IsNullOrEmpty( Format.Path ) )
            {
                return;
            }

            var cell = GetHtmlElementFromDescription();
            if ( cell == null )
            {
                Value = null;
                return;
            }

            if ( Format.ValueFormat == null )
            {
                Value = cell.InnerText;
                return;
            }

            object value;
            if ( Format.ValueFormat.TryConvert( cell.InnerText, out value ) )
            {
                Value = value.ToString();
            }
            else
            {
                Value = cell.InnerText;
            }
        }

        public string ValuesPattern
        {
            get { return myValuesPattern; }
            set
            {
                if ( SetProperty( ref myValuesPattern, value ) )
                {
                    ( (StringContainsLocator)Format.ValuesLocator ).Pattern = myValuesPattern;
                    ValidateValuesLocator();
                }
            }
        }

        public bool IsValid
        {
            get { return myIsValid; }
            set { SetProperty( ref myIsValid, value ); }
        }

        public int TimesPosition
        {
            get { return myTimesPosition; }
            set
            {
                if ( SetProperty( ref myTimesPosition, value ) )
                {
                    ( (AbsolutePositionLocator)Format.TimesLocator ).SeriesPosition = myTimesPosition;
                    ValidateTimesLocator();

                    if ( Format.Orientation == SeriesOrientation.Column )
                    {
                        MarkupBehavior.Marker.RowHeaderColumn = myTimesPosition;
                    }
                    else if ( Format.Orientation == SeriesOrientation.Row )
                    {
                        MarkupBehavior.Marker.ColumnHeaderRow = myTimesPosition;
                    }
                }
            }
        }

        private void ValidateTimesLocator()
        {
            // not yet implemented
        }

        public string SkipValues
        {
            get { return mySkipValues; }
            set
            {
                if ( SetProperty( ref mySkipValues, value ) )
                {
                    Format.Excludes.Clear();
                    Format.Excludes.AddRange( GetIntArray( mySkipValues ) );
                    MarkupBehavior.Marker.SkipRows = Format.Excludes.ToArray();
                }
            }
        }

        private int[] GetIntArray( string value )
        {
            if ( string.IsNullOrWhiteSpace( value ) )
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
