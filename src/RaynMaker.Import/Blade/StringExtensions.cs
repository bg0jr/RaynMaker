using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Returns true if the string contains a "true value"
        /// </summary>
        /// <returns>true if the given string equals (ignoring case): "y", "yes", "on" or "true"</returns>
        public static bool IsTrue( this string value )
        {
            return value != null && ( value.Equals( "true", StringComparison.OrdinalIgnoreCase ) || value.Equals( "y", StringComparison.OrdinalIgnoreCase ) || value.Equals( "yes", StringComparison.OrdinalIgnoreCase ) || value.Equals( "on", StringComparison.OrdinalIgnoreCase ) );
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
        /// Splits the given string into an integer array.
        /// Separator: ,
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int[] ToIntArray( this string s )
        {
            return s.ToIntArray( ',' );
        }
        /// <summary>
        /// Splits the given string into an integer array using
        /// the given separator.
        /// </summary>
        public static int[] ToIntArray( this string s, char separator )
        {
            string[] source = s.Split( new char[]
			{
				separator
			} );
            return (
                from t in source
                where !t.IsNullOrTrimmedEmpty()
                select Convert.ToInt32( t.Trim() ) ).ToArray<int>();
        }
        /// <summary>
        /// Joins the given values with the given separator
        /// </summary>
        public static string Join( this char sep, params string[] values )
        {
            return string.Join( sep.ToString(), values );
        }
        /// <summary>
        /// Removes all occurancies of <c>chrs</c>.
        /// </summary>
        public static string RemoveAll( this string str, params char[] chrs )
        {
            StringBuilder stringBuilder = new StringBuilder();
            char[] array = str.ToCharArray();
            for( int i = 0; i < array.Length; i++ )
            {
                char value = array[ i ];
                if( !chrs.Contains( value ) )
                {
                    stringBuilder.Append( value );
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Adds the given suffix to the string if it does not already ends with the suffix.
        /// </summary>
        public static string AddIfMissing( this string str, string suffix )
        {
            if( !str.EndsWith( suffix, StringComparison.OrdinalIgnoreCase ) )
            {
                str += suffix;
            }
            return str;
        }
        /// <summary />
        public static string TrimSuffix( this string str, string suffix )
        {
            if( str.EndsWith( suffix, StringComparison.OrdinalIgnoreCase ) )
            {
                str = str.Substring( 0, str.Length - suffix.Length );
            }
            return str;
        }
        /// <summary>
        /// Splits the string with the given separators.
        /// </summary>
        public static string[] Split( this string str, params string[] separators )
        {
            return str.Split( separators, StringSplitOptions.None );
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
        /// <summary>
        /// Limits the string to the given number of maximum chars
        /// by cutting at the end if required.
        /// </summary>
        public static string LimitTo( this string str, int maxChars )
        {
            if( str.Length <= maxChars )
            {
                return str;
            }
            return str.Substring( 0, maxChars );
        }
        /// <summary>
        /// Returns all lines contained in string. New line separator is
        /// Environment.NewLine.
        /// </summary>
        public static IEnumerable<string> GetLines( this string str )
        {
            StringReader stringReader = new StringReader( str );
            for( string text = stringReader.ReadLine(); text != null; text = stringReader.ReadLine() )
            {
                yield return text;
            }
            yield break;
        }
        /// <summary>
        /// Wraps the string into '"'.
        /// </summary>
        public static string ToQuoted( this string s )
        {
            return "\"" + s + "\"";
        }
    }
}
