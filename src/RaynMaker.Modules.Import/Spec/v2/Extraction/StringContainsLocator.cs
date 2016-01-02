using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Plainion;
using Plainion.Collections;
using Plainion.Serialization;

namespace RaynMaker.Modules.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Locates a series using substring matching by ignoring the case.
    /// </summary>
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec/v2", Name = "StringContainsLocator" )]
    public class StringContainsLocator : SerializableBindableBase, ISeriesLocator
    {
        private int myHeaderSeriesPosition;
        private string myPattern;

        public StringContainsLocator()
        {
            myHeaderSeriesPosition = -1;
        }

        [Range( 0, int.MaxValue )]
        [DataMember]
        public int HeaderSeriesPosition
        {
            get { return myHeaderSeriesPosition; }
            set { SetProperty( ref myHeaderSeriesPosition, value ); }
        }

        [Required]
        [DataMember]
        public string Pattern
        {
            get { return myPattern; }
            set { SetProperty( ref myPattern, value ); }
        }

        public int FindIndex( IEnumerable<string> list )
        {
            return list.IndexOf( item => item.Contains( Pattern, StringComparison.OrdinalIgnoreCase ) );
        }
    }
}
