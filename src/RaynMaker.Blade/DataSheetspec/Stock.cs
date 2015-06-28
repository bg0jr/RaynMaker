using System.ComponentModel.DataAnnotations;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Stock : Asset
    {
        [Required]
        public string Isin { get; set; }

        [Required, ValidateObject]
        public Overview Overview { get; set; }
    }
}
