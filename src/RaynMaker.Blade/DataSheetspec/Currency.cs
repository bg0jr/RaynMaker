using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;

namespace RaynMaker.Blade.DataSheetSpec
{
    [TypeConverter( typeof( CurrencyConverter ) )]
    [DefaultProperty( "Translations" ), ContentProperty( "Translations" )]
    public class Currency
    {
        public Currency()
        {
            Translations = new ObservableCollection<Translation>();
        }

        [Required]
        public string Name { get; set; }

        public ObservableCollection<Translation> Translations { get; private set; }

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
            return Name == null ? -1 : Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
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
