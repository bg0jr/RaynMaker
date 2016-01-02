using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Plainion;
using Plainion.Collections;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Locates a series using regular expression.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "RegexPatternLocator" )]
    public class RegexPatternLocator : ISeriesLocator
    {
        public RegexPatternLocator( int headerSeriesPosition, string pattern )
            : this( headerSeriesPosition, new Regex( pattern ) )
        {
        }

        public RegexPatternLocator( int headerSeriesPosition, Regex pattern )
        {
            Contract.Requires( headerSeriesPosition >= 0, "HeaderSeriesPosition must be greater or equal to 0" );
            Contract.RequiresNotNull( pattern, "pattern" );
            Contract.RequiresNotNullNotEmpty( pattern.ToString(), "pattern.ToString()" );

            HeaderSeriesPosition = headerSeriesPosition;
            Pattern = pattern;
        }

        [DataMember]
        public int HeaderSeriesPosition { get; private set; }

        public Regex Pattern { get; private set; }

        [DataMember( Name = "Pattern" )]
        private string SerializedPattern
        {
            get { return Pattern == null ? null : Pattern.ToString(); }
            set { Pattern = value == null ? null : new Regex( value ); }
        }

        public int FindIndex( IEnumerable<string> list )
        {
            return list.IndexOf( item => Pattern.IsMatch( item ) );
        }
    }
}
