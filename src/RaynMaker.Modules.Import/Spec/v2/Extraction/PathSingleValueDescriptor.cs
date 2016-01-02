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
        private string myPath;
        private ValueFormat myValueFormat;

        /// <summary>
        /// Gets or sets the path within the document to the figure.
        /// </summary>
        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Path
        {
            get { return myPath; }
            set { SetProperty( ref myPath, value ); }
        }

        [Required, ValidateObject]
        [DataMember]
        public ValueFormat ValueFormat
        {
            get { return myValueFormat; }
            set { SetProperty( ref myValueFormat, value ); }
        }
    }
}
