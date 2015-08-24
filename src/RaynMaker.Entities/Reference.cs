using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace RaynMaker.Entities
{
    [DataContract( Name = "Reference", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Reference : SerializableBindableBase
    {
        private Uri myUri;

        [Required]
        public int Id { get; set; }
        
        [NotMapped]
        [DataMember]
        [Required]
        public Uri Url
        {
            get { return myUri; }
            set { SetProperty( ref myUri, value ); }
        }
    }
}
