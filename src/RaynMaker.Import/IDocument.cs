using System.Data;
using System;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import
{
    public interface IDocument
    {
        string Location { get; }
        DataTable ExtractTable( IFormat format );
    }
}
