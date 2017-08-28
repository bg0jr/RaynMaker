using System;
using System.Collections.Generic;
using System.Linq;
using RaynMaker.Entities;

namespace RaynMaker.Modules.Analysis.UnitTests.Engine.Fakes
{
    class FakeFunctions
    {
        public static double PI()
        {
            return Math.PI;
        }

        public static double Double( double value )
        {
            return value * 2;
        }

        public static double Round( double value, int decimals )
        {
            return Math.Round( value, decimals );   
        }

        public static double Add( double lhs, double rhs )
        {
            return lhs + rhs;
        }

        public static string Substring( string value, double length )
        {
            return value.Substring( 0, ( int )length );
        }

        public static int Count( IEnumerable<IFigure> values )
        {
            return values.Count();
        }
    }
}
