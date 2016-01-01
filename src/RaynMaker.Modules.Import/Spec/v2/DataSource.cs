using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Spec.v2
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "DataSource" )]
    public class DataSource : SpecBase
    {
        private string myVendor;
        private string myName;
        private int myQuality;
        private DocumentType myDocumentType;
        private DocumentLocator myLocatingSpec;

        public DataSource()
        {
            ExtractionSpec = new ObservableCollection<IFigureDescriptor>();
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

        [DataMember]
        public DocumentType DocumentType
        {
            get { return myDocumentType; }
            set { SetProperty( ref myDocumentType, value ); }
        }

        [Required]
        [DataMember]
        public DocumentLocator LocatingSpec
        {
            get { return myLocatingSpec; }
            set { SetProperty( ref myLocatingSpec, value ); }
        }

        [Required, ValidateObject]
        [DataMember]
        public ObservableCollection<IFigureDescriptor> ExtractionSpec { get; private set; }
    }
}
