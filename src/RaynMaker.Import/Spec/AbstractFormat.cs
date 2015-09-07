using System;
using System.Runtime.Serialization;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Base class of all formats.
    /// </summary>
    [Serializable]
    [DataContract( Namespace = "https://github.com/bg0jr/RaynMaker/Import/Spec", Name = "AbstractFormat" )]
    public abstract class AbstractFormat : IFormat
    {
        protected AbstractFormat( string name )
        {
            Name = name;
        }

        public abstract IFormat Clone();

        [DataMember]
        public string Name { get; private set; }
    }
}
