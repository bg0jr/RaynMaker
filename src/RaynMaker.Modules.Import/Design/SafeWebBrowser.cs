using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RaynMaker.Modules.Import.Design
{
    // http://stackoverflow.com/questions/7608550/implement-idispatchinvoke-to-be-called-by-a-webbrowser-control
    public class SafeWebBrowser : WebBrowser
    {
        private const int DISPID_AMBIENT_DLCONTROL = -5512;
        private int myDownloadControlFlags;

        public void LoadHtml( string html )
        {
            Url = new Uri( "about:blank" );

            DocumentText = html;
            Document.OpenNew( true );
            Document.Write( html );

            Refresh();
        }

        protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
        {
            return new SafeWebBrowserSite( this );
        }

        public WebBrowserDownloadControlFlags DownloadControlFlags
        {
            get { return ( WebBrowserDownloadControlFlags )__DownloadControlFlags; }
            set { __DownloadControlFlags = ( int )value; }
        }

        [Browsable( false )]
        [EditorBrowsable( EditorBrowsableState.Never )]
        [DispId( DISPID_AMBIENT_DLCONTROL )]
        public int __DownloadControlFlags
        {
            get { return myDownloadControlFlags; }
            set
            {
                if( myDownloadControlFlags == value )
                {
                    return;
                }

                myDownloadControlFlags = value;

                if( ActiveXInstance == null )
                {
                    return;
                }

                var ctl = ( IOleControl )ActiveXInstance;
                ctl.OnAmbientPropertyChange( DISPID_AMBIENT_DLCONTROL );
            }
        }

        protected class SafeWebBrowserSite : WebBrowserSite, IReflect
        {
            private Dictionary<int, PropertyInfo> myDispIdCache;
            private SafeWebBrowser myHost;

            public SafeWebBrowserSite( SafeWebBrowser host )
                : base( host )
            {
                myHost = host;
            }

            object IReflect.InvokeMember( string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters )
            {
                object ret = null;
                // Check direct IDispatch call using a dispid (see http://msdn.microsoft.com/en-us/library/de3dhzwy.aspx)
                const string dispidToken = "[DISPID=";
                if( name.StartsWith( dispidToken ) )
                {
                    int dispid = int.Parse( name.Substring( dispidToken.Length, name.Length - dispidToken.Length - 1 ) );
                    if( myDispIdCache == null )
                    {
                        // WebBrowser has many properties, so we build a dispid cache on it
                        myDispIdCache = new Dictionary<int, PropertyInfo>();
                        foreach( PropertyInfo pi in myHost.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance ) )
                        {
                            if( ( !pi.CanRead ) || ( pi.GetIndexParameters().Length > 0 ) )
                                continue;

                            object[] atts = pi.GetCustomAttributes( typeof( DispIdAttribute ), true );
                            if( ( atts != null ) && ( atts.Length > 0 ) )
                            {
                                DispIdAttribute da = ( DispIdAttribute )atts[ 0 ];
                                myDispIdCache[ da.Value ] = pi;
                            }
                        }
                    }

                    PropertyInfo property;
                    if( myDispIdCache.TryGetValue( dispid, out property ) )
                    {
                        ret = property.GetValue( myHost, null );
                    }
                }
                return ret;
            }

            FieldInfo[] IReflect.GetFields( BindingFlags bindingAttr )
            {
                return GetType().GetFields( bindingAttr );
            }

            MethodInfo[] IReflect.GetMethods( BindingFlags bindingAttr )
            {
                return GetType().GetMethods( bindingAttr );
            }

            PropertyInfo[] IReflect.GetProperties( BindingFlags bindingAttr )
            {
                return GetType().GetProperties( bindingAttr );
            }

            FieldInfo IReflect.GetField( string name, BindingFlags bindingAttr )
            {
                throw new NotImplementedException();
            }

            MemberInfo[] IReflect.GetMember( string name, BindingFlags bindingAttr )
            {
                throw new NotImplementedException();
            }

            MemberInfo[] IReflect.GetMembers( BindingFlags bindingAttr )
            {
                throw new NotImplementedException();
            }

            MethodInfo IReflect.GetMethod( string name, BindingFlags bindingAttr )
            {
                throw new NotImplementedException();
            }

            MethodInfo IReflect.GetMethod( string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers )
            {
                throw new NotImplementedException();
            }

            PropertyInfo IReflect.GetProperty( string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers )
            {
                throw new NotImplementedException();
            }

            PropertyInfo IReflect.GetProperty( string name, BindingFlags bindingAttr )
            {
                throw new NotImplementedException();
            }

            Type IReflect.UnderlyingSystemType
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
