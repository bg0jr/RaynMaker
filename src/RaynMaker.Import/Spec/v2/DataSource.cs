using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Import.Spec.v2
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DataSource" )]
    public class DataSource
    {
        // e.g. Ariva
        //[Required]
        [DataMember]
        public string Vendor { get; set; }

        // e.g. Fundamentals
        [Required]
        [DataMember]
        public string Name { get; set; }

        //[Required]
        [DataMember]
        public int Quality { get; set; }

        [Required]
        [DataMember]
        public Navigation Location { get; set; }

        [Required, ValidateObject]
        [DataMember]
        public IList<IFormat> ExtractionDescription { get; set; }

        [OnDeserialized]
        private void OnDeserialized( StreamingContext context )
        {
            // make writeable again
            ExtractionDescription = ExtractionDescription.ToList();
        }
    }
}
