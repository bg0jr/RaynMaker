﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities.Datums
{
    [DataContract( Name = "NetIncome", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class NetIncome : AbstractCurrencyDatum
    {
        //[Required]
        public Company Company { get; set; }
    }
}
