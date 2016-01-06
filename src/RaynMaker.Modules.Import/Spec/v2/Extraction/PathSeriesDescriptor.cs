using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a series within a table.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathSeriesDescriptor" )]
    public class PathSeriesDescriptor : SeriesDescriptorBase, IPathDescriptor
    {
        private string myPath;

        /// <summary>
        /// Gets or sets the path within the document to the table.
        /// </summary>
        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Path
        {
            get { return myPath; }
            set { SetProperty( ref myPath, value ); }
        }
    }
}
