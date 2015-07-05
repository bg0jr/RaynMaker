﻿using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public abstract class AnualDatum : Datum, IAnualDatum
    {
        [Required]
        public int Year { get; set; }
    }
}
