using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v1
{
    /// <summary>
    /// Describes range or cell of values of a table.
    /// <remarks>
    /// if both are set we get a cell
    /// </remarks>
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "Anchor" )]
    sealed class Anchor
    {
        private Anchor( ICellLocator row, ICellLocator col )
        {
            Row = row;
            Column = col;
        }

        [DataMember]
        public ICellLocator Column { get; private set; }

        [DataMember]
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
