using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec.Datums
{
    public class Price : DailyDatum, ICurrencyDatum
    {
        [Required]
        public Currency Currency { get; set; }
    }
}
