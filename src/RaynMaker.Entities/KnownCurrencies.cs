using System.Collections.Generic;

namespace RaynMaker.Entities
{
    public class KnownCurrencies
    {
        // http://www.webservicex.net/New/Home/ServiceDetail/10
        public static IReadOnlyDictionary<string, string> All = new Dictionary<string, string>
        {
            {"EUR","Euro"},
            {"USD","U.S. Dollar"},
            {"NOK","Norwegian Krone"},
            {"GBP","British Pound"},
        };
    }
}
