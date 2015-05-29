using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities
{
    public class Company
    {
        public Company()
        {
            Stocks = new List<Stock>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IList<Stock> Stocks { get; private set; }
    }
}
