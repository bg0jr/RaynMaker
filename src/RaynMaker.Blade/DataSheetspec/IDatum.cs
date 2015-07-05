using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public interface IDatum
    {
        [Required]
        DateTime Timestamp { get; }

        [Required]
        double Value { get; }

        [Required]
        string Source { get; }
    }
}
