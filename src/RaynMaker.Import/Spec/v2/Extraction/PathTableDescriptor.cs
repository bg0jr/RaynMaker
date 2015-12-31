using System;
using System.Linq;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes extraction of an entire table which can be located using an explicit path.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathTableDescriptor" )]
    public class PathTableDescriptor : TableDescriptorBase
    {
        public PathTableDescriptor( string figure, string path, params FormatColumn[] cols )
            : base( figure, cols )
        {
            Path = path;
        }

        /// <summary>
        /// Gets or sets the path within the document to the table.
        /// </summary>
        [DataMember]
        public string Path { get; private set; }
    }
}
