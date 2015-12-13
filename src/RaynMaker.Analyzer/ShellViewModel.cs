using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Plainion.AppFw.Wpf.Infrastructure;
using Plainion.AppFw.Wpf.ViewModels;
using Plainion.Prism.Events;
using RaynMaker.Analyzer.Services;
using RaynMaker.Entities;
using RaynMaker.Infrastructure.Events;

namespace RaynMaker.Analyzer
{
    [Export]
    class ShellViewModel : BindableBase
    {
        private const string AppName = "RaynMaker.Analyzer";
        private IProjectService<Project> myProjectService;
        private AssetNavigationService myNavigationService;

        [ImportingConstructor]
        public ShellViewModel( IProjectService<Project> projectService, IEventAggregator eventAggregator,
            AssetNavigationService navigationService )
        {
            myProjectService = projectService;
            myNavigationService = navigationService;

            eventAggregator.GetEvent<ApplicationReadyEvent>().Subscribe( x => OnApplicationReady() );
            eventAggregator.GetEvent<AssetSelectedEvent>().Subscribe( OnAssetSelected );

            AboutCommand = new DelegateCommand( OnAbout );
            HelpCommand = new DelegateCommand( OnHelp );

            ShowLogRequest = new InteractionRequest<INotification>();
            ShowLogCommand = new DelegateCommand( OnShowLog );
        }

        [Import]
        public ProjectLifecycleViewModel<Project> ProjectLifecycleViewModel { get; set; }

        [Import]
        public TitleViewModel<Project> TitleViewModel { get; set; }

        private void OnApplicationReady()
        {
            TitleViewModel.ApplicationName = AppName;

            ProjectLifecycleViewModel.ApplicationName = AppName;
            ProjectLifecycleViewModel.AutoSaveNewProject = true;
            ProjectLifecycleViewModel.FileFilter = "RaynMaker Projects (*.rym)|*.rym";
            ProjectLifecycleViewModel.FileFilterIndex = 0;
            ProjectLifecycleViewModel.DefaultFileExtension = ".rym";

            var args = Environment.GetCommandLineArgs();
            if( args.Length == 2 )
            {
                myProjectService.LoadAsync( args[ 1 ] );
            }

            myNavigationService.NavigateToBrowser();
        }

        public ICommand AboutCommand { get; private set; }

        private void OnAbout()
        {
            Process.Start( "https://github.com/bg0jr/RaynMaker" );
        }

        public ICommand HelpCommand { get; private set; }

        private void OnHelp()
        {
            var home = Path.GetDirectoryName( GetType().Assembly.Location );
            var helpPage = Path.Combine( home, "Help", "Index.html" );
            Process.Start( helpPage );
        }

        private void OnAssetSelected( Stock stock )
        {
            myNavigationService.NavigateToAsset( stock );
        }

        public InteractionRequest<INotification> ShowLogRequest { get; private set; }

        public ICommand ShowLogCommand { get; private set; }

        private void OnShowLog()
        {
            var notification = new Notification();
            notification.Title = "Log";

            ShowLogRequest.Raise( notification, n => { } );
        }
    }
}
