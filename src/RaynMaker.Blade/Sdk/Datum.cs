using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.Sdk
{
    public class Datum<T>
    {
        /// <summary>
        /// Last time value was adapted.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public T Value { get; set; }

        [Required]
        public string Origin { get; set; }
    }
}
