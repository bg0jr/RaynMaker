﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Blade.Entities.Datums
{
    [DataContract( Name = "EBIT", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class EBIT : CurrencyDatum
    {
    }
}