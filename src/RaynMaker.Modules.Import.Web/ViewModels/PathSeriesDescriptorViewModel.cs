using System;
using System.ComponentModel;
using System.Linq;
using Plainion;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class PathSeriesDescriptorViewModel : FigureDescriptorViewModelBase<PathSeriesDescriptor, HtmlTableMarker>
    {
        private string myPath;
        private string myValue;
        private SeriesOrientation mySelectedDimension;
        private bool myIsValid;
        private int myValuesPosition;
        private string myValuesPattern;
        private int myTimesPosition;
        private string mySkipValues;

        public PathSeriesDescriptorViewModel( PathSeriesDescriptor descriptor )
            : this( descriptor, new HtmlMarkupBehavior<HtmlTableMarker>( new HtmlTableMarker() ) )
        {
        }

        /// <summary>
        /// UT only!
        /// </summary>
        internal PathSeriesDescriptorViewModel( PathSeriesDescriptor descriptor, IHtmlMarkupBehavior<HtmlTableMarker> markupBehavior )
            : base( descriptor, markupBehavior )
        {
            Value = "";

            // initialize with invalid values first so that marker gets updated when we set 
            // some valid data from descriptor below
            myValuesPosition = -1;
            myTimesPosition = -1;

            SelectedFigure = Figures.FirstOrDefault( d => d.Name == descriptor.Figure );
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
            TimeFormat = descriptor.TimeFormat;

            IsValid = false;
            if ( descriptor.ValuesLocator != null )
            {
                ValuesPattern = ( (StringContainsLocator)Descriptor.ValuesLocator ).Pattern;
                ValuesPosition = Descriptor.ValuesLocator.HeaderSeriesPosition;
            }
            else
            {
                descriptor.ValuesLocator = new StringContainsLocator();
                ValuesPosition = -1;
                ValuesPattern = null;
            }

            if ( descriptor.TimesLocator != null )
            {
                TimesPosition = ( (AbsolutePositionLocator)Descriptor.TimesLocator ).SeriesPosition;
            }
            else
            {
                Descriptor.TimesLocator = new AbsolutePositionLocator { HeaderSeriesPosition = 0 };
                TimesPosition = -1;
            }

            SelectedOrientation = Descriptor.Orientation;

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
            var cell = MarkupFactory.FindElementByDescriptor( (IHtmlDocument)Document, Descriptor );
            MarkupBehavior.PathToSelectedElement = cell != null ? cell.GetPath().ToString() : null;
        }

        protected override void OnSelectionChanged()
        {
            Path = MarkupBehavior.PathToSelectedElement;

            TryAutoDetectDescriptorFromSelection( MarkupBehavior.SelectedElement );

            ValidateValuesLocator();
            ValidateTimesLocator();
        }

        /// <summary>
        /// Path should come from user click which directly points to the wanted cell.
        /// </summary>
        private void TryAutoDetectDescriptorFromSelection( IHtmlElement selectedElement )
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

            ValuesPattern = ( Descriptor.Orientation == SeriesOrientation.Row
                ? table.GetCell( table.RowIndexOf( selectedElement ), ValuesPosition )
                : table.GetCell( ValuesPosition, table.ColumnIndexOf( selectedElement ) ) )
                .InnerText.Trim();
        }

        public string Path
        {
            get { return myPath; }
            set
            {
                // Path must point to table NOT to cell in table
                var path = value == null ? null : HtmlPath.Parse( value ).GetPathToTable();

                // in case we do not point to table we set the original path
                if ( SetProperty( ref myPath, path != null ? path.ToString() : value ) )
                {
                    Descriptor.Path = myPath;

                    // only overwrite if it was not set yet. Once the user has clicked we want
                    // to keep the user selection and NOT apply the reduced path (which is not supported by HtmlTableMarker)
                    if ( string.IsNullOrEmpty( MarkupBehavior.PathToSelectedElement ) )
                    {
                        MarkupBehavior.PathToSelectedElement = myPath;
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
                SetProperty( ref mySelectedDimension, value );

                // as there is no "None" anylonger we update the expansion all the time
                // so that during initialization the correct values are given to the Marker

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

        public int ValuesPosition
        {
            get { return myValuesPosition; }
            set
            {
                if ( SetProperty( ref myValuesPosition, value ) )
                {
                    ( (StringContainsLocator)Descriptor.ValuesLocator ).HeaderSeriesPosition = myValuesPosition;
                    ValidateValuesLocator();

                    if ( Descriptor.Orientation == SeriesOrientation.Column )
                    {
                        MarkupBehavior.Marker.ColumnHeaderRow = myValuesPosition;
                    }
                    else if ( Descriptor.Orientation == SeriesOrientation.Row )
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
                if ( Descriptor.Orientation == SeriesOrientation.Column )
                {
                    var header = table.GetCell( Descriptor.ValuesLocator.HeaderSeriesPosition, table.RowIndexOf( MarkupBehavior.SelectedElement ) ).InnerText;
                    IsValid = header.Contains( ValuesPattern, StringComparison.OrdinalIgnoreCase );
                }
                else if ( Descriptor.Orientation == SeriesOrientation.Row )
                {
                    var header = table.GetCell( table.RowIndexOf( MarkupBehavior.SelectedElement ), Descriptor.ValuesLocator.HeaderSeriesPosition ).InnerText;
                    IsValid = header.Contains( ValuesPattern, StringComparison.OrdinalIgnoreCase );
                }

                TryUpdateValue();
            }
        }

        private void TryUpdateValue()
        {
            if ( !IsValid || string.IsNullOrEmpty( Descriptor.Path ) )
            {
                return;
            }

            var cell = MarkupFactory.FindElementByDescriptor( (IHtmlDocument)Document, Descriptor );
            if ( cell == null )
            {
                Value = null;
                return;
            }

            if ( Descriptor.ValueFormat == null )
            {
                Value = cell.InnerText;
                return;
            }

            object value;
            if ( Descriptor.ValueFormat.TryConvert( cell.InnerText, out value ) )
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
                    ( (StringContainsLocator)Descriptor.ValuesLocator ).Pattern = myValuesPattern;
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
                    ( (AbsolutePositionLocator)Descriptor.TimesLocator ).SeriesPosition = myTimesPosition;
                    ValidateTimesLocator();

                    if ( Descriptor.Orientation == SeriesOrientation.Column )
                    {
                        MarkupBehavior.Marker.RowHeaderColumn = myTimesPosition;
                    }
                    else if ( Descriptor.Orientation == SeriesOrientation.Row )
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
                    Descriptor.Excludes.Clear();
                    Descriptor.Excludes.AddRange( GetIntArray( mySkipValues ) );

                    if ( Descriptor.Orientation == SeriesOrientation.Row )
                    {
                        MarkupBehavior.Marker.SkipColumns = Descriptor.Excludes.ToArray();
                        MarkupBehavior.Marker.SkipRows = null;
                    }
                    else if ( Descriptor.Orientation == SeriesOrientation.Column )
                    {
                        MarkupBehavior.Marker.SkipColumns = null;
                        MarkupBehavior.Marker.SkipRows = Descriptor.Excludes.ToArray();
                    }
                }
            }
        }

        private int[] GetIntArray( string value )
        {
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

            return new int[] { };
        }

        public FormatColumn TimeFormat { get; private set; }

        public FormatColumn ValueFormat { get; private set; }
    }
}
