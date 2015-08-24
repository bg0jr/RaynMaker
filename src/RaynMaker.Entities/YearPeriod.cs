using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion;

namespace RaynMaker.Entities
{
    [DataContract( Name = "YearPeriod", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    public class YearPeriod : IPeriod
    {
        public YearPeriod( int year )
        {
            Contract.Requires( year > 0, "year must not be negative" );

            Year = year;
        }

        [DataMember]
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
