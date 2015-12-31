using System.Linq;
using System.Runtime.Serialization;
using Plainion;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Defines the properties common to most table describing descriptors
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "TableDescriptorBase" )]
    public abstract class TableDescriptorBase : FigureDescriptorBase
    {
        private int[] mySkipRows = null;
        private int[] mySkipColumns = null;

        protected TableDescriptorBase( string figure, params FormatColumn[] cols )
            : base( figure )
        {
            Contract.RequiresNotNull( cols, "cols" );

            SkipColumns = null;
            SkipRows = null;

            Columns = cols;
        }

        [DataMember]
        public FormatColumn[] Columns { get; private set; }

        [DataMember]
        public int[] SkipRows
        {
            get { return mySkipRows; }
            set { mySkipRows = GetCopyOrEmptySetIfNull( value ); }
        }

        [DataMember]
        public int[] SkipColumns
        {
            get { return mySkipColumns; }
            set { mySkipColumns = GetCopyOrEmptySetIfNull( value ); }
        }

        private int[] GetCopyOrEmptySetIfNull( int[] values )
        {
            if( values == null )
            {
                return new int[] { };
            }

            return values.ToArray();
        }
    }
}
