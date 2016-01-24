using System.Collections.Generic;

namespace RaynMaker.Entities
{
    public class FigureByPeriodComparer : IComparer<IFigure>
    {
        public int Compare( IFigure x, IFigure y )
        {
            return x.Period.CompareTo( y.Period );
        }
    }
}
