using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Entities
{
    [DataContract( Name = "Stock", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( DatumSeries ) )]
    public class Stock : SerializableBindableBase
    {
        private string myIsin;

        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Isin
        {
            get { return myIsin; }
            set { SetProperty( ref myIsin, value ); }
        }

        [DataMember]
        [Required]
        public virtual Company Company { get; set; }
    }
}
