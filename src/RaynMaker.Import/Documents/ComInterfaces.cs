using System;
using System.Runtime.InteropServices;

namespace RaynMaker.Import.Documents
{
    [StructLayout( LayoutKind.Sequential )]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [ComVisible( true ), Guid( "00000118-0000-0000-C000-000000000046" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    public interface IOleClientSite
    {
        [PreserveSig]
        int SaveObject();

        [PreserveSig]
        int GetMoniker( [In, MarshalAs( UnmanagedType.U4 )] int dwAssign, [In, MarshalAs( UnmanagedType.U4 )] int dwWhichMoniker, [MarshalAs( UnmanagedType.Interface )] out object moniker );

        [PreserveSig]
        int GetContainer( out object container );

        [PreserveSig]
        int ShowObject();

        [PreserveSig]
        int OnShowWindow( int show );

        [PreserveSig]
        int RequestNewObjectLayout();
    }

    [ComVisible( true ), ComImport(), Guid( "00000112-0000-0000-C000-000000000046" ), InterfaceTypeAttribute( ComInterfaceType.InterfaceIsIUnknown )]
    public interface IOleObject
    {
        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int SetClientSite( [In, MarshalAs( UnmanagedType.Interface )] IOleClientSite pClientSite );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int GetClientSite( [Out, MarshalAs( UnmanagedType.Interface )] out IOleClientSite site );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int SetHostNames( [In, MarshalAs( UnmanagedType.LPWStr )] String
         szContainerApp, [In, MarshalAs( UnmanagedType.LPWStr )] String
         szContainerObj );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int Close( [In, MarshalAs( UnmanagedType.U4 )] uint dwSaveOption );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int SetMoniker( [In, MarshalAs( UnmanagedType.U4 )] uint dwWhichMoniker, [In,
           MarshalAs( UnmanagedType.Interface )] Object pmk );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int GetMoniker( [In, MarshalAs( UnmanagedType.U4 )] uint dwAssign, [In,
           MarshalAs( UnmanagedType.U4 )] uint dwWhichMoniker, [Out, MarshalAs( UnmanagedType.Interface )] out Object moniker );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int InitFromData( [In, MarshalAs( UnmanagedType.Interface )] Object
         pDataObject, [In, MarshalAs( UnmanagedType.Bool )] Boolean fCreation, [In,
           MarshalAs( UnmanagedType.U4 )] uint dwReserved );

        int GetClipboardData( [In, MarshalAs( UnmanagedType.U4 )] uint dwReserved, out Object data );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int DoVerb( [In, MarshalAs( UnmanagedType.I4 )] int iVerb, [In] IntPtr lpmsg,
           [In, MarshalAs( UnmanagedType.Interface )] IOleClientSite pActiveSite, [In,
             MarshalAs( UnmanagedType.I4 )] int lindex, [In] IntPtr hwndParent, [In] RECT
               lprcPosRect );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int EnumVerbs( out Object e ); // IEnumOLEVERB

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int OleUpdate();

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int IsUpToDate();

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int GetUserClassID( [In, Out] ref Guid pClsid );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int GetUserType( [In, MarshalAs( UnmanagedType.U4 )] uint dwFormOfType, [Out, MarshalAs( UnmanagedType.LPWStr )] out String userType );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int SetExtent( [In, MarshalAs( UnmanagedType.U4 )] uint dwDrawAspect, [In] Object pSizel ); // tagSIZEL

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int GetExtent( [In, MarshalAs( UnmanagedType.U4 )] uint dwDrawAspect, [Out] Object pSizel ); // tagSIZEL

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int Advise( [In, MarshalAs( UnmanagedType.Interface )] System.Runtime.InteropServices.ComTypes.IAdviseSink pAdvSink, out int cookie );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int Unadvise( [In, MarshalAs( UnmanagedType.U4 )] int dwConnection );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int EnumAdvise( out Object e );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int GetMiscStatus( [In, MarshalAs( UnmanagedType.U4 )] uint dwAspect, out int misc );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int SetColorScheme( [In] Object pLogpal ); // tagLOGPALETTE
    }
}
