using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Blade.Data;
using RaynMaker.Import.Core;
using RaynMaker.Import.Spec;
using RaynMaker.Import.Web.ViewModels;
using System.Linq;
using Blade;
using Plainion;
using RaynMaker.Import.Html;

namespace RaynMaker.Import.Web.Views
{
    [Export]
    public partial class WebSpyView : UserControl, IBrowser
    {
        private WebSpyViewModel myViewModel;
        private MarkupDocument myMarkupDocument = null;
        private LegacyDocumentBrowser myDocumentBrowser = null;

        [ImportingConstructor]
        internal WebSpyView( WebSpyViewModel viewModel )
        {
            myViewModel = viewModel;

            InitializeComponent();

            myDocumentBrowser = new LegacyDocumentBrowser( myBrowser );
            myDocumentBrowser.Browser.Navigating += myBrowser_Navigating;
            myDocumentBrowser.Browser.DocumentCompleted += myBrowser_DocumentCompleted;

            // disable links
            // TODO: we cannot use this, it disables navigation in general (Navigate() too)
            //myBrowser.AllowNavigation = false;

            // TODO: how to disable images in browser

            myMarkupDocument = new MarkupDocument();
            myMarkupDocument.ValidationChanged += SeriesName_ValidationChanged;

            myDimension.ItemsSource = Enum.GetValues( typeof( CellDimension ) );

            DataContext = viewModel;

            Loaded += WebSpyView_Loaded;
        }

        private void WebSpyView_Loaded( object sender, RoutedEventArgs e )
        {
            myViewModel.Browser = this;
        }

        private void myBrowser_Navigating( object sender, System.Windows.Forms.WebBrowserNavigatingEventArgs e )
        {
            if( myViewModel.Navigation.IsCapturing )
            {
                myViewModel.Navigation.NavigationUrls += new NavigatorUrl( UriType.Request, e.Url ).ToString();
                myViewModel.Navigation.NavigationUrls += Environment.NewLine;
            }
        }

        private void myBrowser_DocumentCompleted( object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e )
        {
            myPath.Text = "";
            myValue.Text = "";

            if( myMarkupDocument.Document != null )
            {
                myMarkupDocument.Document.Click -= HtmlDocument_Click;
            }

            myMarkupDocument.Document = myDocumentBrowser.Document;
            myMarkupDocument.Document.Click += HtmlDocument_Click;

            myViewModel.AddressBar.Url = myMarkupDocument.Document.Url.ToString();

            if( myViewModel.Navigation.IsCapturing )
            {
                myViewModel.Navigation.NavigationUrls += new NavigatorUrl( UriType.Response, myDocumentBrowser.Browser.Document.Url ).ToString();
                myViewModel.Navigation.NavigationUrls += Environment.NewLine;
            }
        }

        private void HtmlDocument_Click( object sender, System.Windows.Forms.HtmlElementEventArgs e )
        {
            myPath.Text = myMarkupDocument.SelectedElement.GetPath().ToString();
            myValue.Text = myMarkupDocument.SelectedElement.InnerText;
        }

        private void mySearchPath_Click( object sender, RoutedEventArgs e )
        {
            myMarkupDocument.Anchor = myPath.Text;

            if( myMarkupDocument.SelectedElement != null )
            {
                myValue.Text = myMarkupDocument.SelectedElement.InnerText;
            }
        }

        //protected override void OnHandleDestroyed( EventArgs e )
        //{
        //    myMarkupDocument.Dispose();
        //    myMarkupDocument = null;

        //    myDocumentBrowser.Browser.DocumentCompleted -= myBrowser_DocumentCompleted;
        //    myDocumentBrowser.Dispose();
        //    myBrowser.Dispose();
        //    myBrowser = null;

        //    base.OnHandleDestroyed( e );
        //}

        private void myDimension_SelectedIndexChanged( object sender, RoutedEventArgs e )
        {
            myMarkupDocument.Dimension = ( CellDimension )myDimension.SelectedValue;
        }

        private void mySkipRows_TextChanged( object sender, RoutedEventArgs eventArgs )
        {
            SkipElements( mySkipRows, x => myMarkupDocument.SkipRows = x );
        }

        private void mySkipColumns_TextChanged( object sender, RoutedEventArgs eventArgs )
        {
            SkipElements( mySkipColumns, x => myMarkupDocument.SkipColumns = x );
        }

        private void myRowHeader_TextChanged( object sender, RoutedEventArgs e )
        {
            MarkHeader( myRowHeader, x => myMarkupDocument.RowHeader = x );
        }

        private void myColumnHeader_TextChanged( object sender, RoutedEventArgs e )
        {
            MarkHeader( myColumnHeader, x => myMarkupDocument.ColumnHeader = x );
        }

        private void mySeriesName_TextChanged( object sender, RoutedEventArgs eventArgs )
        {
            myMarkupDocument.SeriesName = mySeriesName.Text;
        }

        private void myReset_Click( object sender, RoutedEventArgs e )
        {
            myPath.Text = "";
            myValue.Text = "";

            myDimension.SelectedIndex = 0;

            mySkipColumns.Text = "";
            mySkipRows.Text = "";
            myRowHeader.Text = "";
            myColumnHeader.Text = "";
            mySeriesName.Text = "";

            myMarkupDocument.Reset();
        }

        private void SeriesName_ValidationChanged( bool isValid )
        {
            if( isValid )
            {
                mySeriesName.Background = Brushes.White;
            }
            else
            {
                mySeriesName.Background = Brushes.Red;
            }
        }


        private void MarkHeader( TextBox config, Action<int> UpdateTemplate )
        {
            string str = config.Text.TrimOrNull();
            if( string.IsNullOrEmpty( str ) )
            {
                UpdateTemplate( -1 );
                return;
            }

            CellDimension dimension = ( CellDimension )myDimension.SelectedValue;

            try
            {
                UpdateTemplate( Convert.ToInt32( str ) );
            }
            catch
            {
                //errorProvider1.SetError( config, "Must be: <number> [, <number> ]*" );
            }
        }

        private void SkipElements( TextBox config, Action<int[]> UpdateTemplate )
        {
            if( config.Text.IsNullOrTrimmedEmpty() )
            {
                UpdateTemplate( null );
                return;
            }

            string[] tokens = config.Text.Split( ',' );

            try
            {
                var positions = from t in tokens
                                where !t.IsNullOrTrimmedEmpty()
                                select Convert.ToInt32( t );

                UpdateTemplate( positions.ToArray() );
            }
            catch
            {
                //errorProvider1.SetError( config, "Must be: <number> [, <number> ]*" );
            }
        }

        public void Navigate( string url )
        {
            myDocumentBrowser.Navigate( url );
        }

        public void LoadDocument( IEnumerable<NavigatorUrl> urls )
        {
            myDocumentBrowser.LoadDocument( urls );
        }
    }
}
