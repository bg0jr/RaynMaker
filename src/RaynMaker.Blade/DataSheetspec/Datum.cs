using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Datum : IDatum
    {
        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public double Value { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public IPeriod Period { get; set; }
    }
}
