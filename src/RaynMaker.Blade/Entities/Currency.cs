﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RaynMaker.Blade.Entities
{
    [DataContract( Name = "Currency", Namespace = "https://github.com/bg0jr/RaynMaker" )]
    [KnownType( typeof( Translation ) )]
    [TypeConverter( typeof( CurrencyConverter ) )]
    public class Currency : SerializableBindableBase
    {
        private string myName;

        public Currency()
        {
            Translations = new ObservableCollection<Translation>();
        }

        [DataMember]
        [Required]
        public string Name
        {
            get { return myName; }
            set { SetProperty( ref myName, value ); }
        }

        [DataMember]
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
