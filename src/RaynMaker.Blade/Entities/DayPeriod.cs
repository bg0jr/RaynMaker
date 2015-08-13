using System;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.Entities
{
    public class DayPeriod : IPeriod
    {
        [Required]
        public DateTime Date { get; set; }

        public bool Equals( IPeriod other )
        {
            var otherDay = other as DayPeriod;
            if( otherDay == null )
            {
                return false;
            }

            return Date == otherDay.Date;
        }

        public int CompareTo( IPeriod other )
        {
            var otherDay = other as DayPeriod;
            if( otherDay == null )
            {
                return -2;
            }

            return Date.CompareTo( otherDay.Date );
        }

        public override bool Equals( object obj )
        {
            var other = obj as DayPeriod;
            if( other == null )
            {
                return false;
            }

            return Equals( other );
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode();
        }

        public override string ToString()
        {
            return Date.ToString();
        }
    }
}
