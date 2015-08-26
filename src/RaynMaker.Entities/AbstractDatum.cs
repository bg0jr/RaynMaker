using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaynMaker.Entities
{
    public abstract class AbstractDatum : EntityBase, IDatum
    {
        private DateTime myTimestamp;
        private double? myValue;
        private string mySource;
        private IPeriod myPeriod;

        [Required]
        public long Id { get; set; }

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
