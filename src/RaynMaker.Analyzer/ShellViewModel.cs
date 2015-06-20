using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Plainion.AppFw.Wpf.Infrastructure;
using Plainion.AppFw.Wpf.ViewModels;
using Plainion.Prism.Events;
using RaynMaker.Analyzer.Services;
using RaynMaker.Infrastructure;
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

            AboutCommand = new DelegateCommand( OnAboutCommand );
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
                myProjectService.Load( args[ 1 ] );
            }
        }

        public ICommand AboutCommand { get; private set; }

        private void OnAboutCommand()
        {
            Process.Start( "https://github.com/bg0jr/RaynMaker" );
        }

        private void OnAssetSelected( long assetId )
        {
            myNavigationService.NavigateToAsset( assetId );
        }
    }
}
