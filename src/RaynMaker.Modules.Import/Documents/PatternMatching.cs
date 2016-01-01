using System.Text.RegularExpressions;

namespace RaynMaker.Modules.Import.Documents
{
    /// <summary/>
    public static class PatternMatching
    {
        /// <summary>
        /// Tries to match the given pattern against the given value.
        /// <c>pattern</c> is treated as a 'normal' string with an embedded
        /// regular expression. This embedded regex is marked by surrounding {}.
        /// The non-regex part of <c>pattern</c> is escaped.
        /// </summary>
        /// <returns>null if no embedded regex could be found or the regex didnt match, the 
        /// match result otherwise</returns>
        public static string MatchEmbeddedRegex( string pattern, string value )
        {
            int begin = pattern.IndexOf( '{' );
            int end = pattern.IndexOf( '}' );

            // no embedded regex found
            if ( begin < 0 || end < 0 )
            {
                return null;
            }

            // partition the pattern
            string preRegex = pattern.Substring( 0, begin );
            string embRegex = pattern.Substring( begin + 1, end - begin - 1 );
            string postRegex = pattern.Substring( end + 1 );

            // escape everything before and after the pattern
            pattern = Regex.Escape( preRegex ) + embRegex + Regex.Escape( postRegex );

            Regex regex = new Regex( pattern );
            Match md = regex.Match( value );

            if ( !md.Success )
            {
                return null;
            }

            return md.Groups[ 1 ].Value;
        }
    }
}
