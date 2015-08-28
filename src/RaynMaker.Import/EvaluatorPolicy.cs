using System;
using System.Collections.Generic;
using Blade.Text;

namespace RaynMaker.Import
{
    /// <summary>
    /// Enhances the <see cref="LookupPolicy"/> in that way that all 
    /// strings are evaluated by <see cref="Blade.Text.Evaluator"/>
    /// using the lookup function or LUT provided.
    /// </summary>
    public class EvaluatorPolicy : LookupPolicy
    {
        public EvaluatorPolicy()
            : this( new Dictionary<string, string>() )
        {
        }

        public EvaluatorPolicy( IDictionary<string, string> lut )
            : base( lut )
        {
        }

        public EvaluatorPolicy( Func<string, string> lookup )
            : base( lookup )
        {
        }

        protected override string LookupInternal( string str )
        {
            return Evaluator.Evaluate( str, GetValue );
        }

        /// <summary>
        /// Hook for derived classes to calculate the value for the
        /// given key differently than this class.
        /// </summary>
        protected virtual string GetValue( string key )
        {
            string value = base.LookupInternal( key );
            if ( value == key )
            {
                // could not be evaluted
                return null;
            }

            return value;
        }
    }
}
