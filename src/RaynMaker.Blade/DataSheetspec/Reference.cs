using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Reference
    {
        [Required]
        public Uri Url { get; set; }
    }
}
