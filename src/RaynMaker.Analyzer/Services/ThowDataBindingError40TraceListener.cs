using System;
using System.Diagnostics;

namespace RaynMaker.Analyzer.Services
{
    public class ThowDataBindingError40TraceListener : TraceListener
    {
        private bool myThrowNextError;

        public override void Write( string message )
        {
            myThrowNextError = message.StartsWith( "System.Windows.Data Error: 40 :" );
        }

        public override void WriteLine( string message )
        {
            if ( myThrowNextError )
            {
                throw new ApplicationException( message );
            }
        }

        public static void Initialize()
        {
            PresentationTraceSources.Refresh();

            PresentationTraceSources.DataBindingSource.Listeners.Add(  new ThowDataBindingError40TraceListener() );
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
        }
    }
}
