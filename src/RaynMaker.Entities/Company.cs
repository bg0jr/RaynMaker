using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;

namespace RaynMaker.Entities
{
    [DataContract( Name = "Company", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Reference ) )]
    public class Company : SerializableBindableBase
    {
        private string myName;
        private string myHomepage;
        private string mySector;
        private string myCountry;

        public Company()
        {
            Stocks = new List<Stock>();
        }

        [Required]
        public long Id { get; set; }

        [DataMember]
        [Required]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }

        [DataMember]
        [Required]
        public string Homepage
        {
            get { return myHomepage; }
            set { SetProperty( ref myHomepage, value ); }
        }

        [DataMember]
        public string Sector
        {
            get { return mySector; }
            set { SetProperty( ref mySector, value ); }
        }

        [DataMember]
        public string Country
        {
            get { return myCountry; }
            set { SetProperty( ref myCountry, value ); }
        }

        [DataMember]
        [ValidateObject]
        public ObservableCollection<Reference> References { get; private set; }

        [DataMember]
        [ValidateObject]
        public IList<Stock> Stocks { get; private set; }
    }
}
