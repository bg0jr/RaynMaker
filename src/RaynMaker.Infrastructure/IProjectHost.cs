using System;
using System.ComponentModel.Composition;

namespace RaynMaker.Infrastructure
{
    [InheritedExport( typeof( IProjectHost ) )]
    public interface IProjectHost
    {
        IProject Project { get; }

        event Action Changing;
        event Action Changed;
    }
}
