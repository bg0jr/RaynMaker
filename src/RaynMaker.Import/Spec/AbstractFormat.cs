using System;
using System.Data;
using System.Linq;
using Blade.Data;
using Blade.Reflection;

namespace RaynMaker.Import.Spec
{
    /// <summary>
    /// Base class of all formats.
    /// </summary>
    [Serializable]
    public abstract class AbstractFormat : IFormat
    {
        /// <summary/>
        protected AbstractFormat( string name )
        {
            Name = name;
        }

        /// <summary/>
        protected AbstractFormat( AbstractFormat format, params TransformAction[] rules )
        {
            Name = rules.ApplyTo<string>( () => format.Name );
        }

        /// <summary/>
        public abstract IFormat Clone();

        /// <summary/>
        public string Name { get; private set; }
    }
}
