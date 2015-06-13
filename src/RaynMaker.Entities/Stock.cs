using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities
{
    public class Stock
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Isin { get; set; }
        
        [Required]
        public virtual Company Company { get; set; }
    }
}
