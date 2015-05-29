using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace RaynMaker.Browser
{
    [Export]
    public partial class BrowserView : UserControl
    {
        public BrowserView()
        {
            InitializeComponent();
        }
    }
}
