using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Base class of all formats that describe the whole table instead of a series.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "AbstractTableFormat" )]
    public abstract class AbstractTableFormat : AbstractDimensionalFormat
    {
        protected AbstractTableFormat( string datum, params FormatColumn[] cols )
            : base( datum )
        {
            if( cols == null )
            {
                throw new ArgumentNullException( "cols" );
            }
            Columns = cols;
        }

        [DataMember]
        public FormatColumn[] Columns { get; private set; }
    }
}
