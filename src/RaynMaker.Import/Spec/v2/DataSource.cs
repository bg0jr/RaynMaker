using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Import.Spec.v2.Extraction;
using RaynMaker.Import.Spec.v2.Locating;

namespace RaynMaker.Import.Spec.v2
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "DataSource" )]
    public class DataSource : SpecBase
    {
        private string myVendor;
        private string myName;
        private int myQuality;

        public DataSource()
        {
            ExtractionSpec = new List<IFigureDescriptor>();
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
        public DocumentLocator LocatingSpec { get; set; }

        [Required, ValidateObject]
        [DataMember]
        public IList<IFigureDescriptor> ExtractionSpec { get; private set; }

        [OnDeserialized]
        private void OnDeserialized( StreamingContext context )
        {
            // make writeable again
            ExtractionSpec = ExtractionSpec.ToList();
        }
    }
}
