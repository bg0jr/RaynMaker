using System.Windows;

namespace RaynMaker.Analyzer
{
    public partial class App : Application
    {
        protected override void OnStartup( StartupEventArgs e )
        {
            base.OnStartup( e );

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            new Bootstrapper().Run();
        }
    }
}
