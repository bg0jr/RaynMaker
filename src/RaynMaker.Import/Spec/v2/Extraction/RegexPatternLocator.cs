using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Plainion.Collections;

namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Locates a series using regular expression.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "RegexPatternLocator" )]
    public class RegexPatternLocator : ISeriesLocator
    {
        public RegexPatternLocator( int seriesToScan, string value )
            : this( seriesToScan, new Regex( value ) )
        {
        }

        public RegexPatternLocator( int seriesToScan, Regex value )
        {
            SeriesToScan = seriesToScan;
            Pattern = value;
        }

        [DataMember]
        public int SeriesToScan { get; private set; }

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
