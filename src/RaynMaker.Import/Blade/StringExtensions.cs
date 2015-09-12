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
    }
}
