using System;
using System.Data;
using System.Linq;
using System.Text;
using Blade.Collections;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Base class of all formats that describe the whole table 
    /// instead of a series.
    /// </summary>
    [Serializable]
    public abstract class AbstractTableFormat : AbstractDimensionalFormat
    {
        protected AbstractTableFormat( string name, params FormatColumn[] cols )
            : base( name )
        {
            if ( cols == null )
            {
                throw new ArgumentNullException( "cols" );
            }
            Columns = cols;
        }

        public FormatColumn[] Columns { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( "BaseTableFormat: SkipRows='" );
            sb.Append( SkipRows.Join( ',' ) );
            sb.Append( "', Columns(" );
            for ( int i = 0; i < Columns.Length; ++i )
            {
                sb.Append( "[" );
                sb.Append( Columns[ i ].ToString() );
                sb.Append( "]" );

                if ( i < Columns.Length - 1 )
                {
                    sb.Append( ", " );
                }
            }
            sb.Append( ")" );

            return sb.ToString();
        }

        /// <summary>
        /// Tries to format the given DataTable as descripbed in the 
        /// FormatColumns. That means: types are converted into the 
        /// required types, only the described part of the raw table 
        /// is extracted.
        /// Empty rows will be removed.
        /// </summary>
        public DataTable ToFormattedTable( DataTable rawTable )
        {
            DataTable table = new DataTable();
            Columns.Foreach( col => table.Columns.Add( col.Name, col.Type ) );

            ToFormattedTable( rawTable, table );

            return table;
        }

        public void ToFormattedTable( DataTable rawTable, DataTable targetTable )
        {
            for ( int r = 0; r < rawTable.Rows.Count; ++r )
            {
                if ( SkipRows.Contains( r ) )
                {
                    continue;
                }

                DataRow rawRow = rawTable.Rows[ r ];
                DataRow row = targetTable.NewRow();
                int targetCol = 0;
                bool isEmpty = true;
                for ( int c = 0; c < rawRow.ItemArray.Length; ++c )
                {
                    if ( SkipColumns.Contains( c ) )
                    {
                        continue;
                    }

                    if ( targetCol == Columns.Length )
                    {
                        break;
                    }

                    FormatColumn formatCol = Columns[ targetCol ];
                    object value = formatCol.Convert( rawRow[ c ].ToString() );
                    row[ formatCol.Name ] = ( value != null ? value : DBNull.Value );
                    if ( row[ formatCol.Name ] != DBNull.Value )
                    {
                        isEmpty = false;
                    }

                    targetCol++;
                }

                if ( !isEmpty )
                {
                    targetTable.Rows.Add( row );
                }
            }
        }
    }
}
