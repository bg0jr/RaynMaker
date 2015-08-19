using System;
using System.ComponentModel;
using RaynMaker.Blade.Entities;

namespace RaynMaker.Blade.Entities
{
    [TypeConverter( typeof( PeriodConverter ) )]
    public interface IPeriod : IEquatable<IPeriod>, IComparable<IPeriod>
    {
    }
}
