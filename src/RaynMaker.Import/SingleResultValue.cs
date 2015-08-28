
using RaynMaker.Import.Spec;
namespace RaynMaker.Import
{
    /// <summary>
    /// Represents a single fetched result.
    /// </summary>
    public class SingleResultValue<T>
    {
        public SingleResultValue( Site site, T value )
        {
            Site = site;
            Value = value;
        }

        public T Value { get; private set; }
        public Site Site { get; private set; }
    }
}
