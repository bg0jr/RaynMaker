using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities.Datums
{
    /// <summary>
    /// Total dividend payment. NOT per share
    /// </summary>
    [DataContract( Name = "Dividend", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Dividend : AbstractCurrencyDatum
    {
    }
}
