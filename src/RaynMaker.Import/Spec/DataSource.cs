using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Import.Spec
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "DataSource" )]
    public class DataSource : SpecBase
    {
        private string myVendor;
        private string myName;
        private int myQuality;

        public DataSource()
        {
            FormatSpecs = new List<IFormat>();
        }

        // e.g. Ariva
        [Required]
        [DataMember]
        public string Vendor
        {
            get { return myVendor; }
            set { SetProperty( ref myVendor, value ); }
        }

        // e.g. Fundamentals
        [Required]
        [DataMember]
        public string Name 
                {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }

        [Required]
        [DataMember]
        public int Quality 
        {
            get { return myQuality; }
            set { SetProperty( ref myQuality, value ); }
        }

        [Required]
        [DataMember]
        public Navigation LocationSpec { get; set; }

        [Required, ValidateObject]
        [DataMember]
        public IList<IFormat> FormatSpecs { get; set; }

        [OnDeserialized]
        private void OnDeserialized( StreamingContext context )
        {
            // make writeable again
            FormatSpecs = FormatSpecs.ToList();
        }
    }
}
