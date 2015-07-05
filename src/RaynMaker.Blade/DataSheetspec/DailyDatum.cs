﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class DailyDatum : Datum, IDailyDatum
    {
        [Required]
        public DateTime Date { get; set; }
    }
}
