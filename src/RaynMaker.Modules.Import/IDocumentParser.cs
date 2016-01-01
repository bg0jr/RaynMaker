using System.Data;

namespace RaynMaker.Modules.Import
{
    public interface IDocumentParser
    {
        DataTable ExtractTable();
    }
}
