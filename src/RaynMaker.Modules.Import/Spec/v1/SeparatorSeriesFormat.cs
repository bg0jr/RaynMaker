﻿using System;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v1
{
    /// <summary>
    /// Describes a series format based on a document which
    /// has a table similar structure (e.g. CSV file).
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "SeparatorSeriesFormat" )]
    class SeparatorSeriesFormat : AbstractSeriesFormat
    {
        public SeparatorSeriesFormat( string datum )
            : base( datum )
        {
            Separator = ";";
        }

        /// <summary>
        /// Cell separator used in the file.
        /// </summary>
        [DataMember]
        public string Separator { get; set; }
    }
}
