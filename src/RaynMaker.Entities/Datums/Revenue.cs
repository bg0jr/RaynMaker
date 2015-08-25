using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities.Datums
{
    [DataContract( Name = "Revenue", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class Revenue : AbstractCurrencyDatum
    {
    }
}
