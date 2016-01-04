using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Design
{
    public interface IHtmlMarker
    {
        void Mark( IHtmlElement element );

        void Unmark();

        void Reset();
    }
}
