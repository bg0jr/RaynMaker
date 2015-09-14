using System;
using System.Runtime.InteropServices;

namespace RaynMaker.Import.WinForms
{
    [ComImport, InterfaceType( ComInterfaceType.InterfaceIsIUnknown ), Guid( "B196B288-BAB4-101A-B69C-00AA00341D07" )]
    internal interface IOleControl
    {
        void Reserved0();
        void Reserved1();
        void OnAmbientPropertyChange( int dispID );
        void Reserved2();
    }
}
