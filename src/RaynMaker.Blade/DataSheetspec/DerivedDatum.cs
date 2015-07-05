using System;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class DerivedDatum : ICurrencyDatum, IAnualDatum, IDailyDatum
    {
        public DerivedDatum()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; private set; }

        public double Value { get; set; }

        public DateTime Date { get; set; }

        public int Year { get; set; }

        public Currency Currency { get; set; }

        public string Source { get { return "Calculated"; } }
    }
}
