using System;
using System.ComponentModel;

namespace RaynMaker.Entities
{
    [TypeConverter( typeof( PeriodConverter ) )]
    public interface IPeriod : IEquatable<IPeriod>, IComparable<IPeriod>
    {
    }
}
