using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaynMaker.Entities
{
    public abstract class AbstractFigure : EntityTimestampBase, IFigure
    {
        private double? myValue;
        private string mySource;
        private IPeriod myPeriod;

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
    }
}
