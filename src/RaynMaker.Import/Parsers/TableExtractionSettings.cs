using System;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Spec.v2.Extraction;

namespace RaynMaker.Import.Parsers
{
    /// <summary>
    /// Defines the settings used to extract parts a table.
    /// </summary>
    class TableExtractionSettings
    {
        private Type mySeriesValueType;
        private Type mySeriesHeaderType;
        private int[] mySkipRows;
        private int[] mySkipColumns;
        /// <summary>
        /// If the path points to a cell of a table this property defines whether the cell,
        /// the complete row ore the complete column will be extracted.
        /// <remarks>
        /// Does not have any effect when the path points to the table element
        /// instead of a cell.
        /// </remarks>
        /// </summary>
        public CellDimension Dimension
        {
            get;
            set;
        }
        /// <summary>
        /// Specifies the index of the column which contains the header of the row.
        /// </summary>
        public int RowHeaderColumn
        {
            get;
            set;
        }
        /// <summary>
        /// Specifies the index of the row which contains the header of the column.
        /// </summary>
        public int ColumnHeaderRow
        {
            get;
            set;
        }
        /// <summary>
        /// Specifies rows to skip. 
        /// </summary>
        public int[] SkipRows
        {
            get
            {
                return this.mySkipRows;
            }
            set
            {
                if( value == null )
                {
                    this.mySkipRows = new int[ 0 ];
                    return;
                }
                this.mySkipRows = value;
            }
        }
        /// <summary>
        /// Specifies columns to skip. 
        /// </summary>
        public int[] SkipColumns
        {
            get
            {
                return this.mySkipColumns;
            }
            set
            {
                if( value == null )
                {
                    this.mySkipColumns = new int[ 0 ];
                    return;
                }
                this.mySkipColumns = value;
            }
        }
        /// <summary>
        /// Defines the text the header of the specified column or row must contain(!).
        /// This can be used to validate whether the extracted row/column is the requested one.
        /// Set to null (default) to disable the validation.
        /// <remarks>Only works when one row or one column is extracted. For ExtractColumn
        /// </remarks>
        /// </summary>
        public string SeriesName
        {
            get;
            set;
        }
        /// <summary>
        /// Defines the type of the values of the series.
        /// Default: string
        /// </summary>
        public Type SeriesValueType
        {
            get
            {
                return this.mySeriesValueType;
            }
            set
            {
                this.mySeriesValueType = ( ( value == null ) ? typeof( string ) : value );
            }
        }
        /// <summary>
        /// Defines the type of the header (usually the time axis) of the series.
        /// Default: string
        /// </summary>
        public Type SeriesHeaderType
        {
            get
            {
                return this.mySeriesHeaderType;
            }
            set
            {
                this.mySeriesHeaderType = ( ( value == null ) ? typeof( string ) : value );
            }
        }
        /// <summary>
        /// Defines the defaults.
        /// (extending disabled, skipping disabled, no header validation).
        /// </summary>
        public TableExtractionSettings()
        {
            this.Dimension = CellDimension.None;
            this.SkipColumns = null;
            this.SkipRows = null;
            this.RowHeaderColumn = -1;
            this.ColumnHeaderRow = -1;
            this.SeriesHeaderType = typeof( string );
            this.SeriesValueType = typeof( string );
            this.SeriesName = null;
        }
    }
}
