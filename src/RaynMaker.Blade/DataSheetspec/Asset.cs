using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Plainion.Validation;

namespace RaynMaker.Blade.DataSheetSpec
{
    public abstract class Asset
    {
        public Asset()
        {
            Data = new List<object>();
        }

        [Required]
        public string Name { get; set; }
    
        [ValidateObject]
        public List<object> Data { get; private set; }
    }
}
