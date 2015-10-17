using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Entities
{
    public class Tag : EntityBase
    {
        private string myName;

        [Required]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }

        public override bool Equals( object obj )
        {
            var other = obj as Tag;
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

        public static bool operator ==( Tag lhs, Tag rhs )
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

        public static bool operator !=( Tag lhs, Tag rhs )
        {
            return !( lhs == rhs );
        }
    }
}
