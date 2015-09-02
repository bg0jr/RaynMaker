using System;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Base class of all formats.
    /// </summary>
    [Serializable]
    public abstract class AbstractFormat : IFormat
    {
        protected AbstractFormat( string name )
        {
            Name = name;
        }

        public abstract IFormat Clone();

        public string Name { get; private set; }
    }
}
