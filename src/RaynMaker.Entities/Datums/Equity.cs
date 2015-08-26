﻿using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities.Datums
{
    public class Equity : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}
