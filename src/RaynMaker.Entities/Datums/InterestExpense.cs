﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities.Datums
{
    [DataContract( Name = "InterestExpense", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class InterestExpense : AbstractCurrencyDatum
    {
        [Required]
        public Company Company { get; set; }
    }
}