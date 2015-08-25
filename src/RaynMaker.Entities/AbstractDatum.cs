using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using RaynMaker.Entities;

namespace RaynMaker.Entities
{
    [DataContract( Name = "Datum", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( YearPeriod ) ), KnownType( typeof( DayPeriod ) )]
    public abstract class AbstractDatum : SerializableBindableBase, IDatum
    {
        [DataMember( Name = "Timestamp" )]
        private DateTime myTimestamp;
        private double? myValue;
        private string mySource;
        private IPeriod myPeriod;

        [Required]
        public long Id { get; set; }

        [DataMember]
        [Required]
        public double? Value
        {
            get { return myValue; }
            set
            {
                if( SetProperty( ref myValue, value ) )
                {
                    UpdateTimestamp();
                }
            }
        }

        [DataMember]
        [Required]
        public string Source
        {
            get { return mySource; }
            set { SetProperty( ref mySource, value ); }
        }

        [Required]
        public string RawPeriod
        {
            get { return PeriodConverter.ConvertTo( Period ); }
            set { Period = PeriodConverter.ConvertFrom( value ); }
        }

        [DataMember]
        [NotMapped]
        [Required]
        public IPeriod Period
        {
            get { return myPeriod; }
            set { SetProperty( ref myPeriod, value ); }
        }

        [Required]
        public DateTime Timestamp
        {
            get { return myTimestamp; }
            private set { SetProperty( ref myTimestamp, value ); }
        }

        protected void UpdateTimestamp()
        {
            Timestamp = DateTime.Now;
        }
    }
}
