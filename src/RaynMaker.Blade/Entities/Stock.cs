using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "Stock", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Overview ) )]
    public class Stock : Asset
    {
        private string myIsin;

        public Stock()
        {
            Overview = new Overview();
        }

        [DataMember]
        [Required]
        public string Isin
        {
            get { return myIsin; }
            set { SetProperty( ref myIsin, value ); }
        }

        [DataMember]
        [Required, ValidateObject]
        public Overview Overview { get; private set; }
    }
}
