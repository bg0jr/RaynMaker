using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a position within a table. If Column and Row parameters are specified the described position is a cell otherwise a series (row or column).
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "TableFragmentAnchor" )]
    public sealed class TableFragmentAnchor
    {
        private TableFragmentAnchor( ICellLocator row, ICellLocator col )
        {
            Row = row;
            Column = col;
        }

        [DataMember]
        public ICellLocator Column { get; private set; }

        [DataMember]
        public ICellLocator Row { get; private set; }

        public static TableFragmentAnchor ForRow( ICellLocator row )
        {
            return new TableFragmentAnchor( row, null );
        }

        public static TableFragmentAnchor ForColumn( ICellLocator col )
        {
            return new TableFragmentAnchor( null, col );
        }

        public static TableFragmentAnchor ForCell( ICellLocator row, ICellLocator col )
        {
            return new TableFragmentAnchor( row, col );
        }
    }
}
