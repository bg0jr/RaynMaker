using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Blade.Entities.Datums
{
    [DataContract( Name = "Price", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Price : CurrencyDatum
    {
    }
}
