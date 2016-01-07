using System;
using RaynMaker.Modules.Import.Documents;

namespace RaynMaker.Modules.Import.Design
{
    public interface IHtmlMarkupBehavior<T> : IDisposable where T : IHtmlMarker
    {
        T Marker { get; }

        void AttachTo( IHtmlDocument document );

        void Detach();

        event EventHandler SelectionChanged;

        IHtmlElement SelectedElement { get; set; }

        string PathToSelectedElement { get; set; }
    }
}
