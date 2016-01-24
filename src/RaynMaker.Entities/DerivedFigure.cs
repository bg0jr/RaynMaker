using System;
using System.Collections.Generic;
using RaynMaker.Entities;

namespace RaynMaker.Entities
{
    public class DerivedFigure : IFigure, ICurrencyFigure
    {
        public DerivedFigure()
        {
            Timestamp = DateTime.Now;
            Inputs = new List<IFigure>();
        }

        public DateTime Timestamp { get; private set; }

        public double? Value { get; set; }

        public Currency Currency { get; set; }

        public string Source { get { return "Calculated"; } }

        public List<IFigure> Inputs { get; private set; }

        public IPeriod Period { get; set; }
    }
}
