using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a figure within a document which can be directly located using an explicit path.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "PathSingleValueDescriptor" )]
    public class PathSingleValueDescriptor : FigureDescriptorBase
    {
        /// <summary>
        /// Gets or sets the path within the document to the figure.
        /// </summary>
        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Path { get; set; }

        [Required,ValidateObject]
        [DataMember]
        public ValueFormat ValueFormat { get; set; }
    }
}
