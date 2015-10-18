using System;
using System.Linq;
using RaynMaker.Import.Parsers.Html;
using RaynMaker.Import.Spec;
using Plainion;

namespace RaynMaker.Import.Web.ViewModels
{
    class PathCellFormatViewModel : FormatViewModelBase
    {
        private string myPath;
        private string myValue;
        private int myRowPosition;
        private string myRowPattern;
        private bool myIsRowValid;
        private int myColumnPosition;
        private string myColumnPattern;
        private bool myIsColumnValid;

        public PathCellFormatViewModel( PathCellFormat format )
            : base( format )
        {
            Format = format;

            // essential to make check in UpdateAnchor() work to avoid unintended reset of model 
            // due to half-initialized viewmodel
            myRowPosition = -1;
            myColumnPosition = -1;

            IsRowValid = true;
            IsColumnValid = true;

            Value = "";

            // first set properties without side-effects to others
            SelectedDatum = Datums.FirstOrDefault( d => d.Name == Format.Datum );
            Path = Format.Path;
            ValueFormat = Format.ValueFormat ?? new FormatColumn( "value" );

            if( format.Anchor != null )
            {
                // always first copy patterns - then set position (positions are guard against unintended model overwrite due to half-initialized viewmodel)
                RowPattern = ( ( StringContainsLocator )Format.Anchor.Row ).Pattern;
                ColumnPattern = ( ( StringContainsLocator )Format.Anchor.Column ).Pattern;

                RowPosition = Format.Anchor.Row.SeriesToScan;
                ColumnPosition = Format.Anchor.Column.SeriesToScan;
            }
            else
            {
                RowPosition = -1;
                RowPattern = null;
                ColumnPosition = -1;
                ColumnPattern = null;
            }

            InMillions = Format.InMillions;
        }

        public new PathCellFormat Format { get; private set; }

        protected override void OnSelectionChanged()
        {
            if( MarkupDocument.SelectedElement != null )
            {
                Path = MarkupDocument.SelectedElement.GetPath().ToString();
                Value = MarkupDocument.SelectedElement.InnerText;
                UpdateAnchor();
            }
        }

        public string Path
        {
            get { return myPath; }
            set
            {
                if( SetProperty( ref myPath, value ) )
                {
                    // Path must point to table NOT to cell in table
                    var path = HtmlPath.Parse( myPath );

                    while( path.Elements.Count > 0 )
                    {
                        if( path.PointsToTable )
                        {
                            myPath = path.ToString();
                            break;
                        }

                        path = new HtmlPath( path.Elements.Take( path.Elements.Count - 1 ) );
                    }

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

        public int RowPosition
        {
            get { return myRowPosition; }
            set
            {
                if( SetProperty( ref myRowPosition, value ) )
                {
                    UpdateAnchor();
                    MarkupDocument.RowHeaderColumn = value;
                }
            }
        }

        private void UpdateAnchor()
        {
            // check viewmodel properties here instead of model properties to avoid that half initialized viewmodel overrides model due to property updates
            // (e.g. during initialization in ctor)
            if( RowPosition == -1 || ColumnPosition == -1 )
            {
                return;
            }

            Format.Anchor = Anchor.ForCell( new StringContainsLocator( RowPosition, RowPattern ), new StringContainsLocator( ColumnPosition, ColumnPattern ) );

            if( MarkupDocument.SelectedElement != null )
            {
                if( RowPattern != null )
                {
                    var rowHeader = MarkupDocument.FindRowHeader( Format.Anchor.Row.SeriesToScan )( MarkupDocument.SelectedElement ).InnerText;
                    IsRowValid = rowHeader.Contains( RowPattern, StringComparison.OrdinalIgnoreCase );
                }

                if( ColumnPattern != null )
                {
                    var colHeader = MarkupDocument.FindColumnHeader( Format.Anchor.Column.SeriesToScan )( MarkupDocument.SelectedElement ).InnerText;
                    IsColumnValid = colHeader.Contains( ColumnPattern, StringComparison.OrdinalIgnoreCase );
                }
            }
        }

        public string RowPattern
        {
            get { return myRowPattern; }
            set
            {
                if( SetProperty( ref myRowPattern, value ) )
                {
                    UpdateAnchor();
                }
            }
        }

        public bool IsRowValid
        {
            get { return myIsRowValid; }
            set { SetProperty( ref myIsRowValid, value ); }
        }

        public int ColumnPosition
        {
            get { return myColumnPosition; }
            set
            {
                if( SetProperty( ref myColumnPosition, value ) )
                {
                    UpdateAnchor();
                    MarkupDocument.ColumnHeaderRow = value;
                }
            }
        }

        public string ColumnPattern
        {
            get { return myColumnPattern; }
            set
            {
                if( SetProperty( ref myColumnPattern, value ) )
                {
                    UpdateAnchor();
                }
            }
        }

        public bool IsColumnValid
        {
            get { return myIsColumnValid; }
            set { SetProperty( ref myIsColumnValid, value ); }
        }

        public FormatColumn ValueFormat { get; private set; }
    }
}
