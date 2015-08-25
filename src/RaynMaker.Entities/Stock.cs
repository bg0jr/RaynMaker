using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Plainion.Validation;
using RaynMaker.Entities.Datums;

namespace RaynMaker.Entities
{
    public class Stock : SerializableBindableBase
    {
        private string myIsin;

        public Stock()
        {
            Prices = new List<Price>();
        }

        [Required]
        public long Id { get; set; }

        [Required]
        public string Isin
        {
            get { return myIsin; }
            set { SetProperty( ref myIsin, value ); }
        }

        [Required]
        public virtual Company Company { get; set; }

        [ValidateObject]
        public virtual IList<Price> Prices { get; private set; }
    }
}
