using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Plainion;
using Plainion.Collections;
using Plainion.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Locates a series using regular expression.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "RegexPatternLocator" )]
    public class RegexPatternLocator : SerializableBindableBase, ISeriesLocator
    {
        private int myHeaderSeriesPosition;
        private Regex myPattern;

        [Range( 0, int.MaxValue )]
        [DataMember]
        public int HeaderSeriesPosition
        {
            get { return myHeaderSeriesPosition; }
            set { SetProperty( ref myHeaderSeriesPosition, value ); }
        }

        [Required]
        public Regex Pattern
        {
            get { return myPattern; }
            set { SetProperty( ref myPattern, value ); }
        }

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
