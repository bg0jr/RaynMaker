using System;
using System.Collections.Generic;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class DerivedDatum : IDatum, ICurrencyDatum
    {
        public DerivedDatum()
        {
            Timestamp = DateTime.Now;
            Inputs = new List<IDatum>();
        }

        public DateTime Timestamp { get; private set; }

        public double Value { get; set; }

        public Currency Currency { get; set; }

        public string Source { get { return "Calculated"; } }

        public List<IDatum> Inputs { get; private set; }

        public IPeriod Period { get; set; }
    }
}
