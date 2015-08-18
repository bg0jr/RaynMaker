using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    public class Datum : BindableBase, IDatum
    {
        private DateTime myTimestamp;
        private double myValue;
        private string mySource;
        private IPeriod myPeriod;

        [Required]
        public DateTime Timestamp
        {
            get { return myTimestamp; }
            set { SetProperty( ref myTimestamp, value ); }
        }

        [Required]
        public double Value
        {
            get { return myValue; }
            set { SetProperty( ref myValue, value ); }
        }

        [Required]
        public string Source
        {
            get { return mySource; }
            set { SetProperty( ref mySource, value ); }
        }

        [Required]
        public IPeriod Period
        {
            get { return myPeriod; }
            set { SetProperty( ref myPeriod, value ); }
        }
    }
}
