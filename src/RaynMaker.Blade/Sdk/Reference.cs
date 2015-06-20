using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.Sdk
{
    // TODO: what about derived types giving hint about semantics of the URL like "FinancialReport", "FinancialInfo"
    public class Reference
    {
        [Required]
        public Uri Url { get; set; }
    }
}
