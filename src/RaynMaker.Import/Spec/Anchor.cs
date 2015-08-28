using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Describes range or cell of values of a table.
    /// <remarks>
    /// if both are set we get a cell
    /// </remarks>
    /// </summary>
    public class Anchor
    {
        protected Anchor( ICellLocator row, ICellLocator col )
        {
            Row = row;
            Column = col;
        }

        /// <summary>
        /// Defines how to get a column
        /// </summary>
        public ICellLocator Column { get; private set; }

        /// <summary>
        /// Defines how to get a row
        /// </summary>
        public ICellLocator Row { get; private set; }

        public static Anchor ForRow( ICellLocator row )
        {
            return new Anchor( row, null );
        }

        public static Anchor ForColumn( ICellLocator col )
        {
            return new Anchor( null, col );
        }

        public static Anchor ForCell( ICellLocator row, ICellLocator col )
        {
            return new Anchor( row, col );
        }
    }
}
