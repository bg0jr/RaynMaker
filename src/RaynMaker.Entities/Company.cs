using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Plainion.Validation;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Entities
{
    public class Company : SerializableBindableBase
    {
        private string myName;
        private string myHomepage;
        private string mySector;
        private string myCountry;
        private string myXdbPath;

        public Company()
        {
            Stocks = new List<Stock>();
            References = new ObservableCollection<Reference>();
        }

        [Required]
        public long Id { get; set; }

        [Required]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }

        [Url]
        public string Homepage
        {
            get { return myHomepage; }
            set { SetProperty( ref myHomepage, value ); }
        }

        public string Sector
        {
            get { return mySector; }
            set { SetProperty( ref mySector, value ); }
        }

        public string Country
        {
            get { return myCountry; }
            set { SetProperty( ref myCountry, value ); }
        }

        public string XdbPath
        {
            get { return myXdbPath; }
            set { SetProperty( ref myXdbPath, value ); }
        }

        [ValidateObject]
        public virtual ObservableCollection<Reference> References { get; private set; }

        [ValidateObject]
        public virtual IList<Stock> Stocks { get; private set; }

        [ValidateObject]
        public virtual IList<Assets> Assets { get; private set; }
    }
}
