using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RaynMaker.Import.Documents
{
    [ComVisible( true )]
    class DownloadController : IOleClientSite
    {
        private const int DISPID_AMBIENT_DLCONTROL = -5512;
        private BrowserOptions myOptions;

        [DispId( DISPID_AMBIENT_DLCONTROL )]
        public int IDispatch_Invoke_Handler()
        {
            return ( int )( myOptions );
        }

        public void HookUp( WebBrowserBase webBrowser )
        {
            if( webBrowser == null )
            {
                throw new ArgumentNullException( "webBrowser" );
            }

            IOleObject oleObject = ( IOleObject )webBrowser.ActiveXInstance;
            int rc = oleObject.SetClientSite( this );
            if( rc != 0 )
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
}
