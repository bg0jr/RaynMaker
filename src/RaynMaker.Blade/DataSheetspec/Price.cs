﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Price : DailyDatum, ICurrencyValue
    {
        [Required]
        public Currency Currency { get; set; }
    }
}