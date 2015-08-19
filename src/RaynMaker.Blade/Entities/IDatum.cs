using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.Entities
{
    public interface IDatum
    {
        /// <summary>
        /// Last time value was adapted.
        /// </summary>
        [Required]
        DateTime Timestamp { get; }

        [Required]
        double? Value { get; }

        [Required]
        string Source { get; }

        /// <summary>
        /// The time period this datum applies to.
        /// <seealso cref="YearPeriod"/>
        /// <seealso cref="DayPeriod"/>
        /// </summary>
        [Required]
        IPeriod Period { get; }
    }
}
