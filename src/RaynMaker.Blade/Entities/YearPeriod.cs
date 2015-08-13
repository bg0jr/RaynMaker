using System.ComponentModel.DataAnnotations;
using Plainion;

namespace RaynMaker.Blade.Entities
{
    public class YearPeriod : IPeriod
    {
        public YearPeriod( int year )
        {
            Contract.Requires( year > 0, "year must not be negative" );

            Year = year;
        }

        public int Year { get; private set; }

        public bool Equals( IPeriod other )
        {
            var otherYear = other as YearPeriod;
            if( otherYear == null )
            {
                return false;
            }

            return Year == otherYear.Year;
        }

        public int CompareTo( IPeriod other )
        {
            var otherYear = other as YearPeriod;
            if( otherYear == null )
            {
                return -2;
            }

            return Year.CompareTo( otherYear.Year );
        }

        public override bool Equals( object obj )
        {
            var other = obj as YearPeriod;
            if( other == null )
            {
                return false;
            }

            return Equals( other );
        }

        public override int GetHashCode()
        {
            return Year.GetHashCode();
        }

        public override string ToString()
        {
            return Year.ToString();
        }
    }
}
