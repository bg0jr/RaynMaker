﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Blade.DataSheetSpec.Datums
{
    [DataContract( Name = "Equity", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Equity : CurrencyDatum
    {
    }
}
