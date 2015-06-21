using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Datum
    {
        /// <summary>
        /// Last time value was adapted.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public double Value { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public Currency Currency { get; set; }
    }
}
