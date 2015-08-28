using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms.Integration;
using System.Security;
using System.Windows.Forms;

namespace RaynMaker.Import.Web
{
    /// <summary>
    /// Interaction logic for WebSpy.xaml
    /// </summary>
    public partial class WebSpyWindow : Window
    {
        private WindowsFormsHost myHost;
        private BrowserForm myBrowser;

        public WebSpyWindow()
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

        private void Window_Closed( object sender, EventArgs e )
        {
            myBrowser.Dispose();
        }
    }
}
