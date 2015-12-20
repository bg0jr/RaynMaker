using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Base class of all formats that describe the whole table instead of a series.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "AbstractTableFormat" )]
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

        [DataMember]
        public FormatColumn[] Columns { get; private set; }
    }
}
