using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Blade.DataSheetSpec
{
    [TypeConverter( typeof( CurrencyConverter ) )]
    public class Currency
    {
        public Currency( string name )
        {
            Name = name;
        }

        [Required]
        public string Name { get; private set; }

        public override bool Equals( object obj )
        {
            var other = obj as Currency;
            if( other == null )
            {
                return false;
            }

            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==( Currency lhs, Currency rhs )
        {
            if( object.ReferenceEquals( lhs, null ) && object.ReferenceEquals( rhs, null ) )
            {
                return true;
            }
            if( object.ReferenceEquals( lhs, null ) || object.ReferenceEquals( rhs, null ) )
            {
                return false;
            }

            return lhs.Equals( rhs );
        }

        public static bool operator !=( Currency lhs, Currency rhs )
        {
            return !( lhs == rhs );
        }
    }
}
