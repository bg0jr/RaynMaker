using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Plainion.AppFw.Wpf.Infrastructure;
using Plainion.AppFw.Wpf.ViewModels;
using Plainion.Prism.Events;
using RaynMaker.Entities;
using RaynMaker.Infrastructure;

namespace RaynMaker.Analyzer
{
    [Export]
    class ShellViewModel : BindableBase
    {
        private const string AppName = "RaynMaker.Analyzer";
        private IProjectService<Project> myProjectService;
        private IEntitiesContextFactory myEntitiesContextFactory;

        [ImportingConstructor]
        public ShellViewModel( IProjectService<Project> projectService, IEventAggregator eventAggregator, IEntitiesContextFactory factory )
        {
            myProjectService = projectService;
            myEntitiesContextFactory = factory;

            eventAggregator.GetEvent<ApplicationReadyEvent>().Subscribe( x => OnApplicationReady() );

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
            ProjectLifecycleViewModel.FileFilter = "RaynMaker Analyzer Projects (*.ryma)|*.ryma";
            ProjectLifecycleViewModel.FileFilterIndex = 0;
            ProjectLifecycleViewModel.DefaultFileExtension = ".ryma";

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
    }
}
