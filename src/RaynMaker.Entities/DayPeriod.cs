﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Entities
{
    public class DayPeriod : IPeriod
    {
        public DayPeriod( DateTime day )
        {
            Day = day.Date;
        }

        public DateTime Day { get; set; }

        public bool Equals( IPeriod other )
        {
            var otherDay = other as DayPeriod;
            if( otherDay == null )
            {
                return false;
            }

            return Day == otherDay.Day;
        }

        public int CompareTo( IPeriod other )
        {
            var otherDay = other as DayPeriod;
            if( otherDay == null )
            {
                return -2;
            }

            return Day.CompareTo( otherDay.Day );
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
            return Day.GetHashCode();
        }

        public override string ToString()
        {
            return Day.ToString();
        }
    }
}
