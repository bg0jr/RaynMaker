using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blade;
using Blade.Collections;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Searches with contains. and ignore case
    /// </summary>
    public class StringContainsLocator : ICellLocator
    {
        public StringContainsLocator( int seriesToScan, string value )
        {
            SeriesToScan = seriesToScan;
            Pattern = value;
        }

        public int SeriesToScan { get; private set; }

        public string Pattern { get; private set; }

        public int GetLocation( IEnumerable<string> list )
        {
            return list.IndexOf( item => item.ContainsI( Pattern ) );
        }
    }
}
