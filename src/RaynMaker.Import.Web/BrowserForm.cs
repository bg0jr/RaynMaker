using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Blade;
using Blade.Data;
using RaynMaker.Import;
using RaynMaker.Import.Core;
using RaynMaker.Import.Html;
using RaynMaker.Import.Web.DatumLocationValidation;
using RaynMaker.Import.Spec;

namespace RaynMaker.Import.Web
{
    public partial class BrowserForm : UserControl, IBrowser
    {
        private MarkupDocument myMarkupDocument = null;
        private LegacyDocumentBrowser myDocumentBrowser = null;
        private bool myIsCapturing = false;

        public BrowserForm()
        {
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

            myDimension.DataSource = Enum.GetValues( typeof( CellDimension ) );
        }

        private void myGo_Click( object sender, EventArgs e )
        {
            if ( string.IsNullOrEmpty( myUrlTxt.Text ) )
            {
                return;
            }

            Navigate( myUrlTxt.Text );
        }

        private void myBrowser_Navigating( object sender, WebBrowserNavigatingEventArgs e )
        {
            if ( myIsCapturing )
            {
                myNavUrls.AppendText( new NavigatorUrl( UriType.Request, e.Url ).ToString() );
                myNavUrls.AppendText( Environment.NewLine );
            }
        }

        private void myBrowser_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e )
        {
            myPath.Text = "";
            myValue.Text = "";

            if ( myMarkupDocument.Document != null )
            {
                myMarkupDocument.Document.Click -= HtmlDocument_Click;
            }

            myMarkupDocument.Document = myDocumentBrowser.Document;
            myMarkupDocument.Document.Click += HtmlDocument_Click;

            myUrlTxt.Text = myMarkupDocument.Document.Url.ToString();

            if ( myIsCapturing )
            {
                myNavUrls.AppendText( new NavigatorUrl( UriType.Response, myDocumentBrowser.Browser.Document.Url ).ToString() );
                myNavUrls.AppendText( Environment.NewLine );
            }
        }

        private void HtmlDocument_Click( object sender, HtmlElementEventArgs e )
        {
            myPath.Text = myMarkupDocument.SelectedElement.GetPath().ToString();
            myValue.Text = myMarkupDocument.SelectedElement.InnerText;
        }

        private void mySearchPath_Click( object sender, EventArgs e )
        {
            myMarkupDocument.Anchor = myPath.Text;

            if ( myMarkupDocument.SelectedElement != null )
            {
                myValue.Text = myMarkupDocument.SelectedElement.InnerText;
            }
        }

        protected override void OnHandleDestroyed( EventArgs e )
        {
            myMarkupDocument.Dispose();
            myMarkupDocument = null;

            myDocumentBrowser.Browser.DocumentCompleted -= myBrowser_DocumentCompleted;
            myDocumentBrowser.Dispose();
            myBrowser.Dispose();
            myBrowser = null;

            base.OnHandleDestroyed( e );
        }

        private void myDimension_SelectedIndexChanged( object sender, EventArgs e )
        {
            myMarkupDocument.Dimension = (CellDimension)myDimension.SelectedValue;
        }

        private void mySkipRows_TextChanged( object sender, EventArgs eventArgs )
        {
            SkipElements( mySkipRows, x => myMarkupDocument.SkipRows = x );
        }

        private void mySkipColumns_TextChanged( object sender, EventArgs eventArgs )
        {
            SkipElements( mySkipColumns, x => myMarkupDocument.SkipColumns = x );
        }

        private void myRowHeader_TextChanged( object sender, EventArgs e )
        {
            MarkHeader( myRowHeader, x => myMarkupDocument.RowHeader = x );
        }

        private void myColumnHeader_TextChanged( object sender, EventArgs e )
        {
            MarkHeader( myColumnHeader, x => myMarkupDocument.ColumnHeader = x );
        }

        private void mySeriesName_TextChanged( object sender, EventArgs eventArgs )
        {
            myMarkupDocument.SeriesName = mySeriesName.Text;
        }

        private void myReset_Click( object sender, EventArgs e )
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
            if ( isValid )
            {
                mySeriesName.BackColor = Color.White;
            }
            else
            {
                mySeriesName.BackColor = Color.Red;
            }
        }


        private void MarkHeader( TextBox config, Action<int> UpdateTemplate )
        {
            string str = config.Text.TrimOrNull();
            if ( string.IsNullOrEmpty( str ) )
            {
                UpdateTemplate( -1 );
                return;
            }

            CellDimension dimension = (CellDimension)myDimension.SelectedValue;

            try
            {
                UpdateTemplate( Convert.ToInt32( str ) );
            }
            catch
            {
                errorProvider1.SetError( config, "Must be: <number> [, <number> ]*" );
            }
        }

        private void SkipElements( TextBox config, Action<int[]> UpdateTemplate )
        {
            if ( config.Text.IsNullOrTrimmedEmpty() )
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
                errorProvider1.SetError( config, "Must be: <number> [, <number> ]*" );
            }
        }

        private void myCapture_Click( object sender, EventArgs e )
        {
            myIsCapturing = !myIsCapturing;

            if ( myIsCapturing )
            {
                myCapture.Text = "Stop capturing";
            }
            else
            {
                myCapture.Text = "Start capturing";
            }

            myReplay.Enabled = !myIsCapturing;
        }

        private IList<NavigatorUrl> GetNavigationSteps()
        {
            var q = from token in myNavUrls.Text.Split( new string[] { Environment.NewLine }, StringSplitOptions.None )
                    where !token.IsNullOrTrimmedEmpty()
                    select NavigatorUrl.Parse( token );

            // the last url must be a request
            return (from navUrl in q
                    where !(navUrl == q.Last() && navUrl.UrlType == UriType.Response)
                    select navUrl).ToList();
        }

        private void myReplay_Click( object sender, EventArgs e )
        {
            var q = GetNavigationSteps();

            Regex macroPattern = new Regex( @"(\$\{.*\})" );
            List<NavigatorUrl> filtered = new List<NavigatorUrl>();
            foreach ( NavigatorUrl navUrl in q )
            {
                Match md = macroPattern.Match( navUrl.UrlString );
                if ( md.Success )
                {
                    string macro = md.Groups[ 1 ].Value;
                    string value = InputForm.Show( "Enter macro value", "Enter value for macro " + macro );
                    if ( value != null )
                    {
                        filtered.Add( new NavigatorUrl( navUrl.UrlType, navUrl.UrlString.Replace( macro, value ) ) );
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    filtered.Add( navUrl );
                }
            }

            myDocumentBrowser.LoadDocument( filtered );

            /*
Request: http://www.ariva.de/search/search.m?searchname=${stock.Symbol}&url=/quote/profile.m
Response: http://www.ariva.de/quote/profile.m?secu={(\d+)}
Request: http://www.ariva.de/statistics/facunda.m?secu={0}&page=-1             
             */
        }

        private void myEditCapture_Click( object sender, EventArgs e )
        {
            EditCaptureForm form = new EditCaptureForm();
            form.NavUrls = myNavUrls.Text;
            DialogResult result = form.ShowDialog();

            if ( result == DialogResult.OK )
            {
                myNavUrls.Text = form.NavUrls;
            }
        }

        private void myValidateDatumLocatorsMenu_Click( object sender, EventArgs e )
        {
            var form = new ValidationForm( this );
            form.Show();
        }

        public void Navigate( string url )
        {
            Cursor.Current = Cursors.WaitCursor;
            myDocumentBrowser.Navigate( url );
            Cursor.Current = Cursors.Default;
        }
    }
}