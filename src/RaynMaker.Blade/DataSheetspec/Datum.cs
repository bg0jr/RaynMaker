using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.DataSheetSpec
{
    [DataContract( Name = "Datum", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( YearPeriod ) ), KnownType( typeof( DayPeriod ) )]
    public class Datum : SerializableBindableBase, IDatum
    {
        private DateTime myTimestamp;
        private double myValue;
        private string mySource;
        private IPeriod myPeriod;

        [DataMember]
        [Required]
        public DateTime Timestamp
        {
            get { return myTimestamp; }
            set { SetProperty( ref myTimestamp, value ); }
        }

        [DataMember]
        [Required]
        public double Value
        {
            get { return myValue; }
            set { SetProperty( ref myValue, value ); }
        }

        [DataMember]
        [Required]
        public string Source
        {
            get { return mySource; }
            set { SetProperty( ref mySource, value ); }
        }

        [DataMember]
        [Required]
        public IPeriod Period
        {
            get { return myPeriod; }
            set { SetProperty( ref myPeriod, value ); }
        }
    }
}
