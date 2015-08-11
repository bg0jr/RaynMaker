using System;
using System.ComponentModel;

namespace RaynMaker.Blade.DataSheetSpec
{
    [TypeConverter( typeof( PeriodConverter ) )]
    public interface IPeriod : IEquatable<IPeriod>, IComparable<IPeriod>
    {
    }
}
