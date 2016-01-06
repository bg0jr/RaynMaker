using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Defines the properties common to most single value describing descriptors
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "SingleValueDescriptorBase" )]
    public abstract class SingleValueDescriptorBase : FigureDescriptorBase
    {
        private ValueFormat myValueFormat;

        [Required, ValidateObject]
        [DataMember]
        public ValueFormat ValueFormat
        {
            get { return myValueFormat; }
            set { SetProperty( ref myValueFormat, value ); }
        }
    }
}
