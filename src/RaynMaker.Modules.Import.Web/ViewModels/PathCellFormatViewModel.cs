using System;
using System.Linq;
using RaynMaker.Modules.Import.Parsers.Html;
using RaynMaker.Modules.Import.Spec;
using Plainion;
using RaynMaker.Entities;
using System.Collections.Generic;
using RaynMaker.Infrastructure.Services;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class PathCellFormatViewModel : FormatViewModelBase
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

        public PathCellFormatViewModel( ILutService lutService, PathCellDescriptor format )
            : base( format )
        {
            myLutService = lutService;

            Format = format;

            // essential to make check in UpdateAnchor() work to avoid unintended reset of model 
            // due to half-initialized viewmodel
            myRowPosition = -1;
            myColumnPosition = -1;

            IsRowValid = true;
            IsColumnValid = true;

            Value = "";

            // first set properties without side-effects to others
            SelectedDatum = Datums.FirstOrDefault( d => d.Name == Format.Figure );
            Path = Format.Path;
            ValueFormat = Format.ValueFormat ?? new ValueFormat( typeof( double ) );
            SelectedCurrency = myLutService.CurrenciesLut.Currencies.SingleOrDefault( c => c.Symbol == format.Currency );

            if( format.Column != null )
            {
                // always first copy patterns - then set position (positions are guard against unintended model overwrite due to half-initialized viewmodel)
                ColumnPattern = ( ( StringContainsLocator )Format.Column ).Pattern;
                ColumnPosition = Format.Column.HeaderSeriesPosition;
            }
            else
            {
                ColumnPosition = -1;
                ColumnPattern = null;
            }

            if( format.Row != null )
            {
                // always first copy patterns - then set position (positions are guard against unintended model overwrite due to half-initialized viewmodel)
                RowPattern = ( ( StringContainsLocator )Format.Row ).Pattern;
                RowPosition = Format.Row.HeaderSeriesPosition;
            }
            else
            {
                RowPosition = -1;
                RowPattern = null;
            }

            InMillions = Format.InMillions;
        }

        public new PathCellDescriptor Format { get; private set; }

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

        // TODO: should be enough to only update explicitly what has changed - row or column
        private void UpdateAnchor()
        {
            // check viewmodel properties here instead of model properties to avoid that half initialized viewmodel overrides model due to property updates
            // (e.g. during initialization in ctor)
            if( RowPosition == -1 || ColumnPosition == -1 )
            {
                return;
            }

            Format.Row = new StringContainsLocator( RowPosition, RowPattern );
            Format.Column = new StringContainsLocator( ColumnPosition, ColumnPattern );

            if( MarkupDocument.SelectedElement != null )
            {
                if( RowPattern != null )
                {
                    var rowHeader = MarkupDocument.FindRowHeader( Format.Row.HeaderSeriesPosition )( MarkupDocument.SelectedElement ).InnerText;
                    IsRowValid = rowHeader.Contains( RowPattern, StringComparison.OrdinalIgnoreCase );
                }

                if( ColumnPattern != null )
                {
                    var colHeader = MarkupDocument.FindColumnHeader( Format.Column.HeaderSeriesPosition )( MarkupDocument.SelectedElement ).InnerText;
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
