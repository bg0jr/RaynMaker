using System;
using System.Runtime.InteropServices;

namespace RaynMaker.Import.Documents
{
    //[CLSCompliant(false)]
    [StructLayout( LayoutKind.Sequential )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes" )]
    public struct RECT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields" )]
        public int left;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields" )]
        public int top;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields" )]
        public int right;

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields" )]
        public int bottom;
    }

    //[CLSCompliant( false )]
    [ComVisible( true ), Guid( "00000118-0000-0000-C000-000000000046" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    public interface IOleClientSite
    {
        [PreserveSig]
        int SaveObject();

        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1007:UseGenericsWhereAppropriate" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int GetMoniker( [In, MarshalAs( UnmanagedType.U4 )] int dwAssign, [In, MarshalAs( UnmanagedType.U4 )] int dwWhichMoniker, [MarshalAs( UnmanagedType.Interface )] out object moniker );

        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1007:UseGenericsWhereAppropriate" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int GetContainer( out object container );

        [PreserveSig]
        int ShowObject();

        [PreserveSig]
        int OnShowWindow( int show );

        [PreserveSig]
        int RequestNewObjectLayout();
    }

    //[CLSCompliant( false )]
    [ComVisible( true ), ComImport(),
    Guid( "00000112-0000-0000-C000-000000000046" ),
    InterfaceTypeAttribute( ComInterfaceType.InterfaceIsIUnknown )]
    public interface IOleObject
    {
        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int SetClientSite( [In, MarshalAs( UnmanagedType.Interface )] IOleClientSite pClientSite );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        int GetClientSite( [Out, MarshalAs( UnmanagedType.Interface )] out IOleClientSite site );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int SetHostNames( [In, MarshalAs( UnmanagedType.LPWStr )] String
         szContainerApp, [In, MarshalAs( UnmanagedType.LPWStr )] String
         szContainerObj );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int Close( [In, MarshalAs( UnmanagedType.U4 )] uint dwSaveOption );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int SetMoniker( [In, MarshalAs( UnmanagedType.U4 )] uint dwWhichMoniker, [In,
           MarshalAs( UnmanagedType.Interface )] Object pmk );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1007:UseGenericsWhereAppropriate" )]
        int GetMoniker( [In, MarshalAs( UnmanagedType.U4 )] uint dwAssign, [In,
           MarshalAs( UnmanagedType.U4 )] uint dwWhichMoniker, [Out, MarshalAs( UnmanagedType.Interface )] out Object moniker );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720" )]
        int InitFromData( [In, MarshalAs( UnmanagedType.Interface )] Object
         pDataObject, [In, MarshalAs( UnmanagedType.Bool )] Boolean fCreation, [In,
           MarshalAs( UnmanagedType.U4 )] uint dwReserved );

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1007:UseGenericsWhereAppropriate" )]
        int GetClipboardData( [In, MarshalAs( UnmanagedType.U4 )] uint dwReserved, out Object data );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int DoVerb( [In, MarshalAs( UnmanagedType.I4 )] int iVerb, [In] IntPtr lpmsg,
           [In, MarshalAs( UnmanagedType.Interface )] IOleClientSite pActiveSite, [In,
             MarshalAs( UnmanagedType.I4 )] int lindex, [In] IntPtr hwndParent, [In] RECT
               lprcPosRect );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1007:UseGenericsWhereAppropriate" )]
        int EnumVerbs( out Object e ); // IEnumOLEVERB

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int OleUpdate();

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        int IsUpToDate();

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        int GetUserClassID( [In, Out] ref Guid pClsid );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        int GetUserType( [In, MarshalAs( UnmanagedType.U4 )] uint dwFormOfType, [Out, MarshalAs( UnmanagedType.LPWStr )] out String userType );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int SetExtent( [In, MarshalAs( UnmanagedType.U4 )] uint dwDrawAspect, [In] Object pSizel ); // tagSIZEL

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        int GetExtent( [In, MarshalAs( UnmanagedType.U4 )] uint dwDrawAspect, [Out] Object pSizel ); // tagSIZEL

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        int Advise( [In, MarshalAs( UnmanagedType.Interface )] System.Runtime.InteropServices.ComTypes.IAdviseSink pAdvSink, out int cookie );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int Unadvise( [In, MarshalAs( UnmanagedType.U4 )] int dwConnection );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1007:UseGenericsWhereAppropriate" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        int EnumAdvise( out Object e );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters" )]
        int GetMiscStatus( [In, MarshalAs( UnmanagedType.U4 )] uint dwAspect, out int misc );

        [return: MarshalAs( UnmanagedType.I4 )]
        [PreserveSig]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704" )]
        int SetColorScheme( [In] Object pLogpal ); // tagLOGPALETTE
    }
}
