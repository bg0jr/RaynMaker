using System;
using System.ComponentModel;
using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Entities
{
    [TypeConverter( typeof( PeriodConverter ) )]
    public interface IPeriod : IEquatable<IPeriod>, IComparable<IPeriod>
    {
    }
}
