using System.Linq;
using System.Reflection;
using Plainion;

namespace RaynMaker.Analysis.AnalysisSpec
{
    public class Operators
    {
        public static readonly Operator Greater = new Operator( ">", ( a, b ) => a > b );
        public static readonly Operator GreaterOrEqual = new Operator( ">=", ( a, b ) => a >= b );
        public static readonly Operator Equal = new Operator( "==", ( a, b ) => a == b );
        public static readonly Operator LessOrEqual = new Operator( "<=", ( a, b ) => a <= b );
        public static readonly Operator Less = new Operator( "<", ( a, b ) => a < b );

        internal static Operator Parse( string text )
        {
            var value = typeof( Operators ).GetFields( BindingFlags.Static | BindingFlags.Public )
                .FirstOrDefault( f => f.Name == text );

            Contract.Requires( value != null, "No operator found with name: " + text );

            return ( Operator )value.GetValue( null );
        }
    }
}
