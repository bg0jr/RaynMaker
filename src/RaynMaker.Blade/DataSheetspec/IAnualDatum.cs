using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public interface IAnualDatum : IDatum
    {
        [Required]
        int Year { get; set; }
    }
}
