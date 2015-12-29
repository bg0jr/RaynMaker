﻿using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Base class of all single or multi dimensional formats.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "AbstractDimensionalFormat" )]
    public abstract class AbstractDimensionalFormat : AbstractFormat
    {
        private int[] mySkipRows = null;
        private int[] mySkipColumns = null;

        protected AbstractDimensionalFormat( string datum )
            : base( datum )
        {
            SkipColumns = null;
            SkipRows = null;
        }

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
            if ( values == null )
            {
                return new int[] { };
            }

            return values.ToArray();
        }
    }
}
