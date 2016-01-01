using System;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a series within a table.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathSeriesDescriptor" )]
    public class PathSeriesDescriptor : SeriesDescriptorBase
    {
        public PathSeriesDescriptor( string figure )
            : base( figure )
        {
        }

        /// <summary>
        /// Gets or sets the path within the document to the table.
        /// </summary>
        [DataMember]
        public string Path { get; set; }
    }
}
