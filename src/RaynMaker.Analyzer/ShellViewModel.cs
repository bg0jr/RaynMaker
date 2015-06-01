using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using RaynMaker.Entities;

namespace RaynMaker.Analyzer
{
    [Export]
    class ShellViewModel : BindableBase
    {
        private IEntitiesContextFactory myEntitiesContextFactory;

        [ImportingConstructor]
        public ShellViewModel(IEntitiesContextFactory factory)
        {
            myEntitiesContextFactory = factory;

            NewCommand = new DelegateCommand( OnNewCommand );
            OpenCommand = new DelegateCommand( OnOpenCommand );
            CloseCommand = new DelegateCommand( () => Application.Current.Shutdown() );
            AboutCommand = new DelegateCommand( OnAboutCommand );
        }

        public ICommand NewCommand { get; private set; }

        private void OnNewCommand()
        {
            // just a test
            myEntitiesContextFactory.Create( @"c:\temp\test.db" );
        }
        
        public ICommand OpenCommand { get; private set; }

        private void OnOpenCommand()
        {
        }

        public ICommand CloseCommand { get; private set; }

        public ICommand AboutCommand { get; private set; }
    
        private void OnAboutCommand()
        {
            Process.Start( "https://github.com/bg0jr/RaynMaker" );
        }
    }
}
