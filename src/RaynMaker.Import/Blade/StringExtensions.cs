using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Blade
{
    /// <summary>
    /// General extensions to System.String.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Trims the given string. Returns null if the given string is null.
        /// </summary>
        public static string TrimOrNull( this string value )
        {
            if( value == null )
            {
                return null;
            }
            return value.Trim();
        }

        /// <summary>
        /// Checks whether the string is null or empty after
        /// having been trimmed.
        /// </summary>
        public static bool IsNullOrTrimmedEmpty( this string value )
        {
            return value == null || value.Trim().Length == 0;
        }

        /// <summary>
        /// Parses the given string into the specified enum type.
        /// </summary>
        public static T ToEnum<T>( this string str )
        {
            return ( T )( ( object )Enum.Parse( typeof( T ), str, true ) );
        }

        /// <summary>
        /// Returns all chars before a given separator.
        /// If the separator is not found the whole string
        /// is returned.
        /// </summary>
        public static string TakeBefore( this string s, char separator )
        {
            int num = s.IndexOf( separator );
            if( num >= 0 )
            {
                return s.Substring( 0, num );
            }
            return s;
        }

        /// <summary>
        /// Compares strings ignoring case.
        /// </summary>
        public static bool EqualsI( this string lhs, string rhs )
        {
            return lhs.Equals( rhs, StringComparison.OrdinalIgnoreCase );
        }

        /// <summary>
        /// Implements contains with options
        /// </summary>
        public static bool ContainsI( this string str, string substr )
        {
            return str.ToLower().Contains( substr.ToLower() );
        }
    }
}
