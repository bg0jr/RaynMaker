using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Plainion;
using Plainion.Collections;

namespace RaynMaker.Modules.Import.Spec.v1
{
    /// <summary>
    /// Searches with contains. and ignore case
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "StringContainsLocator" )]
    public class StringContainsLocator : ICellLocator
    {
        public StringContainsLocator( int seriesToScan, string value )
        {
            SeriesToScan = seriesToScan;
            Pattern = value;
        }

        [DataMember]
        public int SeriesToScan { get; private set; }

        [DataMember]
        public string Pattern { get; private set; }

        public int GetLocation( IEnumerable<string> list )
        {
            return list.IndexOf( item => item.Contains( Pattern, StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
