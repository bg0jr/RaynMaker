using System.Runtime.Serialization;
using Plainion;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a position within a table. If Column and Row parameters are specified the described position is a cell otherwise a series (row or column).
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "TableFragmentDescriptor" )]
    public sealed class TableFragmentDescriptor
    {
        private TableFragmentDescriptor( ISeriesLocator row, ISeriesLocator col )
        {
            Row = row;
            Column = col;
        }

        [DataMember]
        public ISeriesLocator Column { get; private set; }

        [DataMember]
        public ISeriesLocator Row { get; private set; }

        // TODO: actually we no longer need "expand" if we have an anchor
        [DataMember]
        public CellDimension Expand { get; private set; }

        public static TableFragmentDescriptor ForRow( ISeriesLocator row )
        {
            Contract.RequiresNotNull( row, "row" );

            return new TableFragmentDescriptor( row, null ) { Expand = CellDimension.Row };
        }

        public static TableFragmentDescriptor ForColumn( ISeriesLocator col )
        {
            Contract.RequiresNotNull( col, "col" );

            return new TableFragmentDescriptor( null, col ) { Expand = CellDimension.Column };
        }

        public static TableFragmentDescriptor ForCell( ISeriesLocator row, ISeriesLocator col )
        {
            Contract.RequiresNotNull( row, "row" );
            Contract.RequiresNotNull( col, "col" );

            return new TableFragmentDescriptor( row, col ) { Expand = CellDimension.None };
        }
    }
}
