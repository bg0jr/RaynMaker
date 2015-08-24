using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.Prism.Interactivity.InteractionRequest;
using Plainion.Xaml;
using RaynMaker.Blade.AnalysisSpec;
using RaynMaker.Blade.Entities;
using RaynMaker.Blade.Model;
using RaynMaker.Blade.Services;
using RaynMaker.Infrastructure;

namespace RaynMaker.Blade
{
    [Export]
    class ShellViewModel : BindableBase
    {
        private IProjectHost myProjectHost;
        private StorageService myStorageService;

        [ImportingConstructor]
        public ShellViewModel( IProjectHost projectHost, Project project, StorageService storageService )
        {
            myProjectHost = projectHost;
            Project = project;
            myStorageService = storageService;

            PropertyChangedEventManager.AddHandler( Project, OnProjectPropertyChanged, string.Empty );

            GoCommand = new DelegateCommand( OnGo, CanGo );

            BrowseDataSheetLocationCommand = new DelegateCommand( OnBrowseDataSheetLocation );
            BrowseDataSheetLocationRequest = new InteractionRequest<OpenFileDialogNotification>();
            EditDataSheetCommand = new DelegateCommand( OnEditDataSheet );
            EditDataSheetRequest = new InteractionRequest<INotification>();

            projectHost.Changed += projectHost_Changed;
            projectHost_Changed();
        }

        void projectHost_Changed()
        {
            if( myProjectHost.Project != null )
            {
                Project.CurrenciesSheet = myStorageService.LoadCurrencies();
            }
        }

        private void OnProjectPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( e.PropertyName == PropertySupport.ExtractPropertyName( () => Project.DataSheetLocation ) )
            {
                GoCommand.RaiseCanExecuteChanged();
            }
        }

        public Project Project { get; private set; }

        public DelegateCommand GoCommand { get; private set; }

        private bool CanGo()
        {
            return myProjectHost.Project != null && Exists( Project.DataSheetLocation );
        }

        private bool Exists( string path )
        {
            return !string.IsNullOrEmpty( path ) && File.Exists( path );
        }

        private void OnGo()
        {
            var analysisTemplate = myStorageService.LoadAnalysisTemplate( Project.CurrenciesSheet );
            var dataSheet = myStorageService.LoadDataSheet( Project.DataSheetLocation );

            if( dataSheet.Asset is Stock )
            {
                var analyzer = new StockAnalyzer( Project, analysisTemplate.Analysis );
                analyzer.Execute( ( Stock )dataSheet.Asset );
            }
            else
            {
                throw new NotSupportedException( "Asset type not supported: " + dataSheet.Asset.GetType() );
            }
        }

        public DelegateCommand BrowseDataSheetLocationCommand { get; private set; }

        private void OnBrowseDataSheetLocation()
        {
            var notification = new OpenFileDialogNotification();
            notification.RestoreDirectory = true;
            notification.Filter = "XDB files (*.xdb)|*.xdb";
            notification.FilterIndex = 0;
            notification.MultiSelect = false;

            BrowseDataSheetLocationRequest.Raise( notification,
                n =>
                {
                    if( n.Confirmed )
                    {
                        Project.DataSheetLocation = n.FileName;
                    }
                } );
        }

        public InteractionRequest<OpenFileDialogNotification> BrowseDataSheetLocationRequest { get; private set; }

        public DelegateCommand EditDataSheetCommand { get; private set; }

        private void OnEditDataSheet()
        {
            var notification = new Notification();
            notification.Title = "Datasheet";

            EditDataSheetRequest.Raise( notification, n => { } );
        }

        public InteractionRequest<INotification> EditDataSheetRequest { get; private set; }
    }
}
