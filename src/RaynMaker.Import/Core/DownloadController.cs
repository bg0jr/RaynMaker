using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using RaynMaker.Import;

namespace RaynMaker.Import.Core
{
    //[CLSCompliant( false )]
    [ComVisible( true )]
    public class DownloadController : IOleClientSite, IDownloadController
    {
        private const int DISPID_AMBIENT_DLCONTROL = -5512;
        private BrowserOptions myOptions;

        [DispId( DISPID_AMBIENT_DLCONTROL )]
        public virtual int IDispatchInvokeHandler()
        {

            return (int)(myOptions);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes" )]
        public void HookUp( WebBrowserBase webBrowser )
        {
            if ( webBrowser == null )
            {
                throw new ArgumentNullException( "webBrowser" );
            }

            IOleObject oleObject = (IOleObject)webBrowser.ActiveXInstance;
            int rc = oleObject.SetClientSite( this );
            if ( rc != 0 )
            {
                throw new COMException( "Unknown COM error", rc );
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes" )]
        public void HookUp( IOleObject obj )
        {
            if ( obj == null )
            {
                throw new ArgumentNullException( "obj" );
            }

            int rc = obj.SetClientSite( this );
            if ( rc != 0 )
            {
                throw new COMException( "Unknown COM error", rc );
            }
        }

        public BrowserOptions Options
        {
            get { return myOptions; }
            set { myOptions = value; }
        }

        #region IOleClientSite Members

        public int SaveObject()
        {
            return 0;
        }

        public int GetMoniker( int dwAssign, int dwWhichMoniker, out object moniker )
        {
            moniker = null;

            return 0;
        }

        public int GetContainer( out object container )
        {
            container = null;

            return 0;
        }

        public int ShowObject()
        {
            return 0;
        }

        public int OnShowWindow( int show )
        {
            return 0;
        }

        public int RequestNewObjectLayout()
        {
            return 0;
        }

        #endregion
    }

    /*
    [ComVisible( true )]
    public partial class WebBrowserControl : UserControl, IOleClientSite
    {
        private BrowserOptions _options;
        public WebBrowserControl()
        {
            InitializeComponent();
        }


        protected override void OnLoad( EventArgs e )
        {
            IOleObject oc = (IOleObject)webBrowser.ActiveXInstance;
            oc.SetClientSite( this );
        }


        public BrowserOptions Options
        {
            get
            {
                return this._options;
            }
            set
            {
                this._options = value;
            }
        }

        public WebBrowser Browser
        {
            get { return webBrowser; }
        }

        [DispId( -5512 )]
        public virtual int IDispatch_Invoke_Handler()
        {
            return (int)0;
            //return (int)this._options;
        }

        #region IOleClientSite Members

        public int SaveObject()
        {
            return 0;
        }

        public int GetMoniker( int dwAssign, int dwWhichMoniker, out object moniker )
        {
            moniker = this;
            return 0;
        }

        public int GetContainer( out object container )
        {
            container = this;
            return 0;
        }

        public int ShowObject()
        {
            return 0;
        }

        public int OnShowWindow( int fShow )
        {
            return 0;
        }

        public int RequestNewObjectLayout()
        {
            return 0;
        }

        #endregion
    }




    partial class WebBrowserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            //
            // webBrowser
            //
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point( 0, 0 );
            this.webBrowser.MinimumSize = new System.Drawing.Size( 20, 20 );
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size( 150, 150 );
            this.webBrowser.TabIndex = 0;
            //
            // WebBrowserControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.webBrowser );
            this.Name = "WebBrowserControl";
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
    }
    */
}
