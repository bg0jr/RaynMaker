using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion.Validation;
using RaynMaker.Entities;

namespace RaynMaker.Entities
{
    [DataContract( Name = "Overview", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Reference ) )]
    public class Overview : SerializableBindableBase
    {
        private string myHomepage;
        private string mySector;
        private string myCountry;

        public Overview()
        {
            References = new ObservableCollection<Reference>();
        }

        [Required]
        public int Id { get; set; }

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

        [DataMember( Name = "Origin" )]
        public string Country
        {
            get { return myCountry; }
            set { SetProperty( ref myCountry, value ); }
        }

        [DataMember]
        [ValidateObject]
        public ObservableCollection<Reference> References { get; private set; }
    }
}
