using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace RaynMaker.Analyzer
{
    [Export]
    class ShellViewModel : BindableBase
    {
        public ShellViewModel()
        {
            NewCommand = new DelegateCommand( OnNewCommand );
            OpenCommand = new DelegateCommand( OnOpenCommand );
            CloseCommand = new DelegateCommand( () => Application.Current.Shutdown() );
            AboutCommand = new DelegateCommand( OnAboutCommand );
        }

        public ICommand NewCommand { get; private set; }

        private void OnNewCommand()
        {

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
