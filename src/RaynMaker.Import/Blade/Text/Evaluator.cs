using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.Text
{
    /// <summary>
    /// Simple text evaluator. Recognizes placeholders and replaces them 
    /// with values in place.
    /// <remarks>
    /// Syntax of the placeholder is <c>${placeholder}</c>.
    /// </remarks>
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// Searches for placeholder, calls the <c>ValueEval</c> to get a value 
        /// for it and replaces the placeholder with the given value.
        /// </summary>
        public static string Evaluate( string s, Func<string, string> ValueEval )
        {
            StringBuilder stringBuilder = new StringBuilder();
            int i;
            int num2;
            for( i = 0; i < s.Length; i = num2 + 1 )
            {
                int num = s.IndexOf( "${", i );
                if( num == -1 )
                {
                    break;
                }
                num2 = s.IndexOf( "}", num );
                if( num2 == -1 )
                {
                    break;
                }
                string text = s.Substring( num + 2, num2 - num - 2 );
                stringBuilder.Append( s.Substring( i, num - i ) );
                string text2 = ValueEval( text );
                if( text2 == null )
                {
                    throw new InvalidOperationException( "Could not resolve variable: " + text );
                }
                stringBuilder.Append( text2 );
            }
            if( i > 0 )
            {
                stringBuilder.Append( s.Substring( i ) );
            }
            if( i != 0 )
            {
                return stringBuilder.ToString();
            }
            return s;
        }
    }
}
