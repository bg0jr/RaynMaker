using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public interface IDailyDatum : IDatum
    {
        [Required]
        DateTime Date { get; set; }
    }
}
