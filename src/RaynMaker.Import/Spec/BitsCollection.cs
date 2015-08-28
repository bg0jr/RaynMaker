using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RaynMaker.Import.Spec
{
    public class BitsCollection : ArrayList
    {
        public override int Add( object value )
        {
            var doc = value as Document;
            if ( doc != null )
            {
                foreach ( var bit in doc.Bits )
                {
                    base.Add( bit );
                }

                return Count - 1;
            }
            else
            {
                return base.Add( value );
            }
        }
    }

}

