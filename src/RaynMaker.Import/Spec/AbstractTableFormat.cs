using System;
using System.Data;
using System.Linq;
using System.Text;

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
            if( cols == null )
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
            sb.Append( string.Join( ",", SkipRows ) );
            sb.Append( "', Columns(" );
            for( int i = 0; i < Columns.Length; ++i )
            {
                sb.Append( "[" );
                sb.Append( Columns[ i ].ToString() );
                sb.Append( "]" );

                if( i < Columns.Length - 1 )
                {
                    sb.Append( ", " );
                }
            }
            sb.Append( ")" );

            return sb.ToString();
        }
    }
}
