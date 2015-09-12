using System;
using System.Windows.Forms;

namespace RaynMaker.Import.Web
{
    public interface IBrowser : IBrowserBase
    {
        event Action<Uri> Navigating;

        event Action<HtmlDocument> DocumentCompleted;
    }
}
