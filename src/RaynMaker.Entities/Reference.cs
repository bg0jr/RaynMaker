using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace RaynMaker.Entities
{
    [DataContract( Name = "Reference", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Reference : SerializableBindableBase
    {
        private string myUri;

        [Required]
        public int Id { get; set; }
        
        [DataMember]
        [Required,Url]
        public string Url
        {
            get { return myUri; }
            set { SetProperty( ref myUri, value ); }
        }
    }
}
