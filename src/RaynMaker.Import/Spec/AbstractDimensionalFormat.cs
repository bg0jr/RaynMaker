using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Base class of all single or multi dimensional formats.
    /// </summary>
    [Serializable]
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "AbstractDimensionalFormat" )]
    public abstract class AbstractDimensionalFormat : AbstractFormat
    {
        private int[] mySkipRows = null;
        private int[] mySkipColumns = null;

        /// <summary/>
        protected AbstractDimensionalFormat( string name )
            : base( name )
        {
            SkipColumns = null;
            SkipRows = null;
        }

        /// <summary>
        /// Rows to skip while parsing input.
        /// </summary>
        [DataMember]
        public int[] SkipRows
        {
            get { return mySkipRows; }
            set { mySkipRows = GetCopyOrEmptySetIfNull( value ); }
        }

        /// <summary>
        /// Columns to skip while parsing input.
        /// </summary>
        [DataMember]
        public int[] SkipColumns
        {
            get { return mySkipColumns; }
            set { mySkipColumns = GetCopyOrEmptySetIfNull( value ); }
        }

        private int[] GetCopyOrEmptySetIfNull( int[] values )
        {
            if ( values == null )
            {
                return new int[] { };
            }

            return values.ToArray();
        }
    }
}
