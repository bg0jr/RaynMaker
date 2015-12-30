﻿using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a position within a table. If Column and Row parameters are specified the described position is a cell otherwise a series (row or column).
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "Anchor" )]
    public sealed class TableCellAnchor
    {
        private TableCellAnchor( ICellLocator row, ICellLocator col )
        {
            Row = row;
            Column = col;
        }

        [DataMember]
        public ICellLocator Column { get; private set; }

        [DataMember]
        public ICellLocator Row { get; private set; }

        public static TableCellAnchor ForRow( ICellLocator row )
        {
            return new TableCellAnchor( row, null );
        }

        public static TableCellAnchor ForColumn( ICellLocator col )
        {
            return new TableCellAnchor( null, col );
        }

        public static TableCellAnchor ForCell( ICellLocator row, ICellLocator col )
        {
            return new TableCellAnchor( row, col );
        }
    }
}
