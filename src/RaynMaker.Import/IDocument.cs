using System;
using System.Data;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public interface IDocument
    {
        Uri Location { get; }
    }
}
