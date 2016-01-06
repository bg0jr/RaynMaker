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
    class PathCellFormatViewModel : FormatViewModelBase<HtmlTableMarker>
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

        // guard to avoid unintended reset of model due to half-initialized viewmodel 
        private bool myAllowUpdateModel;

        public PathCellFormatViewModel( ILutService lutService, PathCellDescriptor descriptor )
            : base( descriptor, new HtmlTableMarker() )
        {
            myLutService = lutService;

            Format = descriptor;

            myAllowUpdateModel = false;

            Value = "";

            SelectedDatum = Datums.FirstOrDefault( d => d.Name == Format.Figure );
            Path = Format.Path;
            ValueFormat = Format.ValueFormat ?? new ValueFormat( typeof( double ) );
            SelectedCurrency = myLutService.CurrenciesLut.Currencies.SingleOrDefault( c => c.Symbol == descriptor.Currency );
            InMillions = Format.InMillions;

            IsColumnValid = false;
            if( descriptor.Column != null )
            {
                ColumnPattern = ( ( StringContainsLocator )Format.Column ).Pattern;
                ColumnPosition = Format.Column.HeaderSeriesPosition;
            }
            else
            {
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
                RowPosition = -1;
                RowPattern = null;
            }

            myAllowUpdateModel = true;
            UpdateRow();
            UpdateColumn();
        }

        public new PathCellDescriptor Format { get; private set; }

        protected override void OnSelectionChanged()
        {
            if( MarkupBehavior.SelectedElement != null )
            {
                Path = MarkupBehavior.PathToSelectedElement;
                Value = MarkupBehavior.SelectedElement.InnerText;

                UpdateRow();
                UpdateColumn();
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
                    var path = HtmlPath.Parse( myPath ).GetPathToTable();

                    Format.Path = myPath;

                    if( !string.IsNullOrWhiteSpace( myPath ) )
                    {
                        MarkupBehavior.PathToSelectedElement = myPath;

                        if( MarkupBehavior.SelectedElement != null )
                        {
                            Value = MarkupBehavior.SelectedElement.InnerText;
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
                    UpdateRow();
                    MarkupBehavior.Marker.RowHeaderColumn = value;
                }
            }
        }

        private void UpdateRow()
        {
            if( !myAllowUpdateModel )
            {
                return;
            }

            Format.Row = new StringContainsLocator { HeaderSeriesPosition = RowPosition, Pattern = RowPattern };

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
                    UpdateRow();
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
                    UpdateColumn();
                    MarkupBehavior.Marker.ColumnHeaderRow = value;
                }
            }
        }

        private void UpdateColumn()
        {
            if( !myAllowUpdateModel )
            {
                return;
            }

            Format.Column = new StringContainsLocator { HeaderSeriesPosition = ColumnPosition, Pattern = ColumnPattern };

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
                    UpdateColumn();
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
