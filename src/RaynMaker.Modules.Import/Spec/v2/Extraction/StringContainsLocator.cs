using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Plainion;
using Plainion.Collections;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Locates a series using substring matching by ignoring the case.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "StringContainsLocator" )]
    public class StringContainsLocator : ISeriesLocator
    {
        public StringContainsLocator( int headerSeriesPosition, string pattern )
        {
            Contract.Requires( headerSeriesPosition >= 0, "HeaderSeriesPosition must be greater or equal to 0" );
            Contract.RequiresNotNullNotEmpty( pattern, "pattern" );
            
            HeaderSeriesPosition = headerSeriesPosition;
            Pattern = pattern;
        }

        [DataMember]
        public int HeaderSeriesPosition { get; private set; }

        [DataMember]
        public string Pattern { get; private set; }

        public int FindIndex( IEnumerable<string> list )
        {
            return list.IndexOf( item => item.Contains( Pattern, StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
