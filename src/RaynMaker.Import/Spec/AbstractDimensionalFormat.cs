using System;
using System.Data;
using System.Linq;
using Blade.Data;
using Blade.Reflection;
using Blade.Collections;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Base class of all single or multi dimensional formats.
    /// </summary>
    [Serializable]
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

        /// <summary/>
        protected AbstractDimensionalFormat( AbstractDimensionalFormat format, params TransformAction[] rules )
            : base( format, rules )
        {
            SkipColumns = rules.ApplyTo<int[]>( () => format.SkipColumns );
            SkipRows = rules.ApplyTo<int[]>( () => format.SkipRows );
        }

        /// <summary>
        /// Rows to skip while parsing input.
        /// </summary>
        public int[] SkipRows
        {
            get { return mySkipRows; }
            set { mySkipRows = GetCopyOrEmptySetIfNull( value ); }
        }

        /// <summary>
        /// Columns to skip while parsing input.
        /// </summary>
        public int[] SkipColumns
        {
            get { return mySkipColumns; }
            set { mySkipColumns = GetCopyOrEmptySetIfNull( value ); }
        }

        /// <summary/>
        protected int[] GetCopyOrEmptySetIfNull( int[] values )
        {
            if ( values == null )
            {
                return new int[] { };
            }

            return values.ToArray();
        }
    }
}
