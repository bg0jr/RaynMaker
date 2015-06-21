using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public abstract class Asset
    {
        [Required]
        public string Name { get; set; }
    }
}
