using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Documents;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class PathCellDescriptorViewModel : FigureDescriptorViewModelBase<PathCellDescriptor, HtmlTableMarker>
    {
        private ILutService myLutService;
        private string myPath;
        private string myValue;
        private int myRowPosition;
        private string myRowPattern;
        private bool myIsRowValid;
        private int myColumnPosition;
        private string myColumnPattern;
        private bool myIsColumnValid;
        private Currency mySelectedCurreny;

        public PathCellDescriptorViewModel( ILutService lutService, PathCellDescriptor descriptor )
            : this( lutService, descriptor, new HtmlMarkupBehavior<HtmlTableMarker>( new HtmlTableMarker() ) )
        {
        }

        /// <summary>
        /// UT only!
        /// </summary>
        internal PathCellDescriptorViewModel( ILutService lutService, PathCellDescriptor descriptor, IHtmlMarkupBehavior<HtmlTableMarker> markupBehavior )
            : base( descriptor, markupBehavior )
        {
            myLutService = lutService;

            Value = "";

            // initialize with invalid values first so that marker gets updated when we set 
            // some valid data from descriptor below
            myColumnPosition= -1;
            myRowPosition = -1;

            SelectedDatum = Datums.FirstOrDefault( d => d.Name == Descriptor.Figure );
            Path = Descriptor.Path;

            if( Descriptor.ValueFormat == null )
            {
                Descriptor.ValueFormat = new ValueFormat( typeof( double ) );
            }
            ValueFormat = Descriptor.ValueFormat;
            PropertyChangedEventManager.AddHandler( ValueFormat, OnValueFormatChanged, string.Empty );

            SelectedCurrency = myLutService.CurrenciesLut.Currencies.SingleOrDefault( c => c.Symbol == descriptor.Currency );

            IsColumnValid = false;
            if( descriptor.Column != null )
            {
                ColumnPattern = ( ( StringContainsLocator )Descriptor.Column ).Pattern;
                ColumnPosition = Descriptor.Column.HeaderSeriesPosition;
            }
            else
            {
                Descriptor.Column = new StringContainsLocator();
                ColumnPosition = -1;
                ColumnPattern = null;
            }

            IsRowValid = false;
            if( descriptor.Row != null )
            {
                RowPattern = ( ( StringContainsLocator )Descriptor.Row ).Pattern;
                RowPosition = Descriptor.Row.HeaderSeriesPosition;
            }
            else
            {
                Descriptor.Row = new StringContainsLocator();
                RowPosition = -1;
                RowPattern = null;
            }

            ValidateRow();
            ValidateColumn();
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

            ValidateRow();
            ValidateColumn();
        }

        /// <summary>
        /// Path should come from user click which directly points to the wanted cell.
        /// </summary>
        private void TryAutoDetectDescriptorFromSelection( IHtmlElement selectedElement )
        {
            if( !selectedElement.GetPath().PointsToTableCell )
            {
                return;
            }

            var table = MarkupBehavior.Marker.Table;
            if( table == null )
            {
                return;
            }

            if( ColumnPosition == -1 )
            {
                // just guess ...
                ColumnPosition = 0;
            }

            if( RowPosition == -1 )
            {
                // just guess ...
                RowPosition = 0;
            }

            ColumnPattern = table.GetCell( ColumnPosition, table.ColumnIndexOf( selectedElement ) ).InnerText.Trim();
            RowPattern = table.GetCell( table.RowIndexOf( selectedElement ), RowPosition ).InnerText.Trim();
        }

        public string Path
        {
            get { return myPath; }
            set
            {
                // Path must point to table NOT to cell in table
                var path = value == null ? null : HtmlPath.Parse( value ).GetPathToTable();

                // in case we do not point to table we set the original path
                if( SetProperty( ref myPath, path != null ? path.ToString() : value ) )
                {
                    Descriptor.Path = myPath;

                    // only overwrite if it was not set yet. Once the user has clicked we want
                    // to keep the user selection and NOT apply the reduced path (which is not supported by HtmlTableMarker)
                    if( string.IsNullOrEmpty( MarkupBehavior.PathToSelectedElement ) )
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

        public int RowPosition
        {
            get { return myRowPosition; }
            set
            {
                if( SetProperty( ref myRowPosition, value ) )
                {
                    ( ( StringContainsLocator )Descriptor.Row ).HeaderSeriesPosition = myRowPosition;
                    ValidateRow();
                    MarkupBehavior.Marker.RowHeaderColumn = value;
                }
            }
        }

        private void ValidateRow()
        {
            var table = MarkupBehavior.Marker.Table;

            if( table != null && RowPattern != null )
            {
                var rowHeader = table.GetCell( table.RowIndexOf( MarkupBehavior.SelectedElement ), Descriptor.Row.HeaderSeriesPosition ).InnerText;
                IsRowValid = rowHeader.Contains( RowPattern, StringComparison.OrdinalIgnoreCase );

                TryUpdateValue();
            }
        }

        private void TryUpdateValue()
        {
            if( !IsRowValid || !IsColumnValid || string.IsNullOrEmpty( Descriptor.Path ) )
            {
                return;
            }

            var cell = MarkupFactory.FindElementByDescriptor( (IHtmlDocument)Document, Descriptor );
            if ( cell == null )
            {
                Value = null;
                return;
            }

            if( Descriptor.ValueFormat == null )
            {
                Value = cell.InnerText;
                return;
            }

            object value;
            if( Descriptor.ValueFormat.TryConvert( cell.InnerText, out value ) )
            {
                Value = value.ToString();
            }
            else
            {
                Value = cell.InnerText;
            }
        }

        public string RowPattern
        {
            get { return myRowPattern; }
            set
            {
                if( SetProperty( ref myRowPattern, value ) )
                {
                    ( ( StringContainsLocator )Descriptor.Row ).Pattern = myRowPattern;
                    ValidateRow();
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
                    ( ( StringContainsLocator )Descriptor.Column ).HeaderSeriesPosition = myColumnPosition;
                    ValidateColumn();
                    MarkupBehavior.Marker.ColumnHeaderRow = value;
                }
            }
        }

        private void ValidateColumn()
        {
            var table = MarkupBehavior.Marker.Table;

            if( table != null && ColumnPattern != null )
            {
                var colHeader = table.GetCell( Descriptor.Column.HeaderSeriesPosition, table.ColumnIndexOf( MarkupBehavior.SelectedElement ) ).InnerText;
                IsColumnValid = colHeader.Contains( ColumnPattern, StringComparison.OrdinalIgnoreCase );

                TryUpdateValue();
            }
        }

        public string ColumnPattern
        {
            get { return myColumnPattern; }
            set
            {
                if( SetProperty( ref myColumnPattern, value ) )
                {
                    ( ( StringContainsLocator )Descriptor.Column ).Pattern = myColumnPattern;
                    ValidateColumn();
                }
            }
        }

        public bool IsColumnValid
        {
            get { return myIsColumnValid; }
            set { SetProperty( ref myIsColumnValid, value ); }
        }

        public ValueFormat ValueFormat { get; private set; }

        public IEnumerable<Currency> Currencies
        {
            get
            {
                // we want to allow to choose "nothing" as currency - simple solution: add "empty currency" ("null")
                return new Currency[] { null }.Concat( myLutService.CurrenciesLut.Currencies );
            }
        }

        public Currency SelectedCurrency
        {
            get { return mySelectedCurreny; }
            set
            {
                if( SetProperty( ref mySelectedCurreny, value ) )
                {
                    Descriptor.Currency = value != null ? value.Symbol : null;
                }
            }
        }
    }
}
