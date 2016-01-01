using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RaynMaker.Modules.Analysis.AnalysisSpec
{
    [TypeConverter( typeof( OperatorConverter ) )]
    public class Operator
    {
        private Func<double, double, bool> myComparer;

        public Operator( string name, Func<double, double, bool> comparer )
        {
            Name = name;
            myComparer = comparer;
        }

        [Required]
        public string Name { get; private set; }

        public bool Compare( double a, double b )
        {
            return myComparer( a, b );
        }
    }
}
