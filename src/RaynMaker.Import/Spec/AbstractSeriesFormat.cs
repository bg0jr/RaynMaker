using System;
using System.Data;
using System.Drawing;
using System.Linq;
using Blade.Data;

namespace RaynMaker.Import.Spec
{
    // TODO: rename celldimension to TableOrientation

    /// <summary>
    /// Base class for all formats that describe a series of data.
    /// A series consists of a set of time-value pairs.
    /// </summary>
    // TODO: actually we no longer need "expand" if we have an anchor
    [Serializable]
    public abstract class AbstractSeriesFormat : AbstractDimensionalFormat
    {
        protected AbstractSeriesFormat( string name )
            : base( name )
        {
            SeriesNamePosition = -1;
            TimeAxisPosition = -1;
        }

        /// <summary>
        /// Creates a deep copy of the given object.
        /// </summary>
        protected void CloneTo( AbstractSeriesFormat other )
        {
            other.Expand = Expand;
            other.SeriesNamePosition = SeriesNamePosition;
            other.TimeAxisPosition = TimeAxisPosition;

            if ( SkipRows != null )
            {
                other.SkipRows = SkipRows.ToArray();
            }
            if ( SkipColumns != null )
            {
                other.SkipColumns = SkipColumns.ToArray();
            }
            if ( ValueFormat != null )
            {
                other.ValueFormat = new FormatColumn( ValueFormat );
            }
            if ( TimeAxisFormat != null )
            {
                other.TimeAxisFormat = new FormatColumn( TimeAxisFormat );
            }
        }

        /// <summary>
        /// Direction of the series in the "table".
        /// </summary>
        public CellDimension Expand { get; set; }

        /// <summary>
        /// Position of the series name: the column
        /// if Expand == Row, the row otherwise.
        /// </summary>
        public int SeriesNamePosition { get; set; }

        /// <summary>
        /// Position of the time axis: the row
        /// if Expand == Row, the column otherwise.
        /// </summary>
        public int TimeAxisPosition { get; set; }

        /// <summary>
        /// Format of the value column.
        /// </summary>
        public FormatColumn ValueFormat { get; set; }

        /// <summary>
        /// Format of the time axis column.
        /// </summary>
        public FormatColumn TimeAxisFormat { get; set; }

        /// <summary>
        /// Defines how to find the position of the series in the table.
        /// </summary>
        public Anchor Anchor { get; set; }

        /// <summary>
        /// Tries to format the given DataTable as descripbed in 
        /// ValueFormat and TimeAxisFormat. That means: types are 
        /// converted into the required types, the first column
        /// is treated as value column the second as timeaxis column.
        /// <remarks>Usually this method is called with a table
        /// which has been tailored using DataTable.ExtractSeries()
        /// </remarks>
        /// </summary>
        public virtual DataTable ToFormattedTable( DataTable table_in )
        {
            if ( table_in == null )
            {
                throw new ArgumentNullException( "table_in" );
            }

            DataTable rawTable = table_in;

            if ( Anchor != null )
            {
                rawTable = ExtractSeries( table_in, Anchor );
                if ( rawTable == null )
                {
                    return null;
                }
            }

            if ( ValueFormat == null && TimeAxisFormat == null )
            {
                return rawTable;
            }

            DataTable table = new DataTable();

            if ( ValueFormat != null )
            {
                table.Columns.Add( new DataColumn( ValueFormat.Name, ValueFormat.Type ) );
            }
            else
            {
                throw new InvalidOperationException( "No value format specified" );
            }

            if ( TimeAxisFormat != null )
            {
                table.Columns.Add( new DataColumn( TimeAxisFormat.Name, TimeAxisFormat.Type ) );
            }

            foreach ( DataRow rawRow in rawTable.Rows )
            {
                DataRow row = table.NewRow();

                Action<int, FormatColumn> Convert = ( idx, col ) =>
                    {
                        object value = ( col == null ? rawRow[ idx ] : col.Convert( rawRow[ idx ].ToString() ) );
                        row[ idx ] = ( value != null ? value : DBNull.Value );
                    };

                Convert( 0, ValueFormat );
                if ( TimeAxisFormat != null )
                {
                    Convert( 1, TimeAxisFormat );
                }

                table.Rows.Add( row );
            }

            return table;
        }

        /// <summary>
        /// SeriesName validation not enabled here.
        /// </summary>
        public TableExtractionSettings ToExtractionSettings()
        {
            TableExtractionSettings settings = new TableExtractionSettings();
            settings.Dimension = Expand;
            if ( Expand == CellDimension.Column )
            {
                settings.ColumnHeaderRow = SeriesNamePosition;
                settings.RowHeaderColumn = TimeAxisPosition;
            }
            else
            {
                settings.ColumnHeaderRow = TimeAxisPosition;
                settings.RowHeaderColumn = SeriesNamePosition;
            }
            settings.SkipColumns = SkipColumns;
            settings.SkipRows = SkipRows;

            // do not enable validation here
            //settings.SeriesName = format.SeriesNameContains;

            return settings;
        }

        protected DataTable ExtractSeries( DataTable rawTable, Anchor anchor_in )
        {
            if ( anchor_in == null )
            {
                throw new ArgumentException( "anchor missing" );
            }

            // calculate the anchor
            Point anchor = new Point( 0, 0 );

            if ( Anchor.Column != null )
            {
                int rowToScan = Anchor.Column.SeriesToScan;
                if ( rawTable.Rows.Count <= rowToScan )
                {
                    throw new InvalidExpressionException( "Anchor points outside table" );
                }

                anchor.X = Anchor.Column.GetLocation( rawTable.Rows[ rowToScan ].ItemArray.Select( item => item.ToString() ) );
                if ( anchor.X == -1 )
                {
                    throw new InvalidExpressionException( "Anchor condition failed: column not found" );
                }
            }

            if ( Anchor.Row != null )
            {
                int colToScan = Anchor.Row.SeriesToScan;
                if ( rawTable.Columns.Count <= colToScan )
                {
                    throw new InvalidExpressionException( "Anchor points outside table" );
                }

                anchor.Y = Anchor.Row.GetLocation( rawTable.Rows.ToSet().Select( row => row[ colToScan ].ToString() ) );
                if ( anchor.Y == -1 )
                {
                    throw new InvalidExpressionException( "Anchor condition failed: row not found" );
                }
            }

            // How to get the value
            Func<object, object> GetValue = value => value;

            return rawTable.ExtractSeries( anchor, GetValue, ToExtractionSettings() );
        }
    }
}
