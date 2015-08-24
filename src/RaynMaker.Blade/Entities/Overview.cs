using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "Overview", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Reference ) )]
    public class Overview
    {
        public Overview()
        {
            References = new ObservableCollection<Reference>();
        }

        [DataMember]
        [Required]
        public string Homepage { get; set; }

        [DataMember]
        public string Sector { get; set; }

        [DataMember]
        public string Origin { get; set; }

        [DataMember]
        [ValidateObject]
        public ObservableCollection<Reference> References { get; private set; }
    }
}
