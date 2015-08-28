using System.ComponentModel.Composition;
using System.Security;
using System.Windows;
using System.Windows.Forms.Integration;

namespace RaynMaker.Import.Web.Views
{
    /// <summary>
    /// Interaction logic for WebSpy.xaml
    /// </summary>
    [Export]
    public partial class WebSpyView : System.Windows.Controls.UserControl
    {
        private WindowsFormsHost myHost;
        private BrowserForm myBrowser;

        public WebSpyView()
        {
            InitializeComponent();
        }

        [SecuritySafeCritical]
        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            myHost = new WindowsFormsHost();
            myHost.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            myHost.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            myHost.Margin = new Thickness( 0 );

            myBrowser = new BrowserForm();

            myHost.Child = myBrowser;

            myContent.Children.Add( myHost );
        }

        //private void Window_Closed( object sender, EventArgs e )
        //{
        //    myBrowser.Dispose();
        //}
    }
}
