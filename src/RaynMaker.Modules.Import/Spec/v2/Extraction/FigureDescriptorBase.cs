using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Defines the properties common to most figure descriptors
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "FigureDescriptorBase" )]
    public abstract class FigureDescriptorBase : IFigureDescriptor
    {
        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Figure { get; set; }

        [DataMember]
        public bool InMillions { get; set; }
    }
}
