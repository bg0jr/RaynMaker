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
    }
}
