using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.Sdk
{
    public abstract class Asset
    {
        [Required]
        public string Name { get; set; }
    }
}
