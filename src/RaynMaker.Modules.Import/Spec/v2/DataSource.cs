using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Serialization;
using Plainion.Validation;
using RaynMaker.Modules.Import.Spec.v2.Extraction;
using RaynMaker.Modules.Import.Spec.v2.Locating;

namespace RaynMaker.Modules.Import.Spec.v2
{
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "DataSource" )]
    public class DataSource : SerializableBindableBase
    {
        private string myVendor;
        private string myName;
        private int myQuality;
        private DocumentType myDocumentType;
        private DocumentLocator myLocation;

        public DataSource()
        {
            Figures = new ObservableCollection<IFigureDescriptor>();
        }

        // e.g. Ariva
        [Required(AllowEmptyStrings=false)]
        [DataMember]
        public string Vendor
        {
            get { return myVendor; }
            set { SetProperty( ref myVendor, value ); }
        }

        // e.g. Fundamentals
        [Required( AllowEmptyStrings = false )]
        [DataMember]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }

        [Range( 0, int.MaxValue )]
        [DataMember]
        public int Quality
        {
            get { return myQuality; }
            set { SetProperty( ref myQuality, value ); }
        }

        [DocumentTypeNotNone]
        [DataMember]
        public DocumentType DocumentType
        {
            get { return myDocumentType; }
            set { SetProperty( ref myDocumentType, value ); }
        }

        [Required]
        [DataMember]
        public DocumentLocator Location
        {
            get { return myLocation; }
            set { SetProperty( ref myLocation, value ); }
        }

        [Required, ValidateObject]
        [DataMember]
        public IList<IFigureDescriptor> Figures { get; private set; }

        [OnDeserialized]
        private void OnDeserialized( StreamingContext context )
        {
            // make writeable again
            Figures = new ObservableCollection<IFigureDescriptor>( Figures );
        }
    }
}
