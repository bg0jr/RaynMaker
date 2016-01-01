using System.Text;
using Plainion;

namespace RaynMaker.Modules.Analysis.Engine
{
    class TextEvaluator
    {
        private ExpressionEvaluator myExpressionEvaluator;

        public TextEvaluator( ExpressionEvaluator evaluator )
        {
            myExpressionEvaluator = evaluator;
        }

        public string Evaluate( string text )
        {
            var sb = new StringBuilder();

            int start = 0;
            while( -1 < start && start < text.Length )
            {
                var pos = text.IndexOf( "${", start );
                if( pos == -1 )
                {
                    sb.Append( text.Substring( start ) );
                    break;
                }

                sb.Append( text.Substring( start, pos - start ) );

                start = pos;

                pos = text.IndexOf( "}", start );
                if( pos == -1 )
                {
                    sb.Append( text.Substring( start, pos - start ) );
                    break;
                }

                var expr = text.Substring( start, pos - start + 1 );
                sb.Append( FormatValue( ProvideValue( expr ) ) );

                start = pos + 1;
            }

            return sb.ToString();
        }

        private string FormatValue( object value )
        {
            if( value == null )
            {
                return "n.a.";
            }
            else if( value is double )
            {
                return ( ( double )value ).ToString( "0.00" );
            }
            else
            {
                return value.ToString();
            }
        }

        public object ProvideValue( string expr )
        {
            Contract.Requires( expr.StartsWith( "${" ) && expr.EndsWith( "}" ), "Not an expression: " + expr );

            var path = expr.Substring( 2, expr.Length - 3 );

            return myExpressionEvaluator.Evaluate( path );
        }
    }
}
