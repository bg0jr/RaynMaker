using System;
using System.Collections.Generic;
using System.Linq;
using Plainion;
using RaynMaker.Entities;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Design;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class PathCellFormatViewModel : FormatViewModelBase<PathCellDescriptor, HtmlTableMarker>
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

        public PathCellFormatViewModel( ILutService lutService, PathCellDescriptor descriptor )
            : base( descriptor, new HtmlTableMarker() )
        {
            myLutService = lutService;

            Value = "";

            SelectedDatum = Datums.FirstOrDefault( d => d.Name == Format.Figure );
            Path = Format.Path;

            if( Format.ValueFormat == null )
            {
                Format.ValueFormat = new ValueFormat( typeof( double ) );
            }
            ValueFormat = Format.ValueFormat;

            SelectedCurrency = myLutService.CurrenciesLut.Currencies.SingleOrDefault( c => c.Symbol == descriptor.Currency );

            IsColumnValid = false;
            if( descriptor.Column != null )
            {
                ColumnPattern = ( ( StringContainsLocator )Format.Column ).Pattern;
                ColumnPosition = Format.Column.HeaderSeriesPosition;
            }
            else
            {
                Format.Column = new StringContainsLocator();
                ColumnPosition = -1;
                ColumnPattern = null;
            }

            IsRowValid = false;
            if( descriptor.Row != null )
            {
                RowPattern = ( ( StringContainsLocator )Format.Row ).Pattern;
                RowPosition = Format.Row.HeaderSeriesPosition;
            }
            else
            {
                Format.Row = new StringContainsLocator();
                RowPosition = -1;
                RowPattern = null;
            }

            ValidateRow();
            ValidateColumn();
        }

        protected override void OnSelectionChanged()
        {
            if( MarkupBehavior.SelectedElement != null )
            {
                Path = MarkupBehavior.PathToSelectedElement;

                ValidateRow();
                ValidateColumn();
            }
        }

        protected override void OnDocumentChanged()
        {
            MarkupBehavior.PathToSelectedElement = Path;
        }

        public string Path
        {
            get { return myPath; }
            set
            {
                // Path must point to table NOT to cell in table
                var path = value == null ? null : HtmlPath.Parse( value ).GetPathToTable().ToString();

                if( SetProperty( ref myPath, path ) )
                {
                    Format.Path = myPath;

                    MarkupBehavior.PathToSelectedElement = myPath;

                    if( MarkupBehavior.SelectedElement != null )
                    {
                        Value = MarkupBehavior.SelectedElement.InnerText;
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
                    ( ( StringContainsLocator )Format.Row ).HeaderSeriesPosition = myRowPosition;
                    ValidateRow();
                    MarkupBehavior.Marker.RowHeaderColumn = value;
                }
            }
        }

        private void ValidateRow()
        {
            if( MarkupBehavior.SelectedElement != null && RowPattern != null )
            {
                var table = MarkupBehavior.Marker.Table;
                var rowHeader = table.GetCellAt( table.GetRowIndex( MarkupBehavior.SelectedElement ), Format.Row.HeaderSeriesPosition ).InnerText;
                IsRowValid = rowHeader.Contains( RowPattern, StringComparison.OrdinalIgnoreCase );
            }
        }

        public string RowPattern
        {
            get { return myRowPattern; }
            set
            {
                if( SetProperty( ref myRowPattern, value ) )
                {
                    ( ( StringContainsLocator )Format.Row ).Pattern = myRowPattern;
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
                    ( ( StringContainsLocator )Format.Column ).HeaderSeriesPosition = myColumnPosition;
                    ValidateColumn();
                    MarkupBehavior.Marker.ColumnHeaderRow = value;
                }
            }
        }

        private void ValidateColumn()
        {
            if( MarkupBehavior.SelectedElement != null && ColumnPattern != null )
            {
                var table = MarkupBehavior.Marker.Table;
                var colHeader = table.GetCellAt( Format.Column.HeaderSeriesPosition, table.GetColumnIndex( MarkupBehavior.SelectedElement ) ).InnerText;
                IsColumnValid = colHeader.Contains( ColumnPattern, StringComparison.OrdinalIgnoreCase );
            }
        }

        public string ColumnPattern
        {
            get { return myColumnPattern; }
            set
            {
                if( SetProperty( ref myColumnPattern, value ) )
                {
                    ( ( StringContainsLocator )Format.Column ).Pattern = myColumnPattern;
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
                    Format.Currency = value != null ? value.Symbol : null;
                }
            }
        }
    }
}
