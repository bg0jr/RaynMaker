using System;

namespace RaynMaker.Blade.Tests.Engine.Fakes
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

        public static double Add( double lhs, double rhs )
        {
            return lhs + rhs;
        }

        public static string Substring( string value, double length )
        {
            return value.Substring( 0, (int)length );
        }
    }
}
