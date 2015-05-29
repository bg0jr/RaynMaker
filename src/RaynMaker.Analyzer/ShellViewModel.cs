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
            NewCommand = new DelegateCommand( () => { } );
            OpenCommand = new DelegateCommand( () => { } );
            CloseCommand = new DelegateCommand( () => Application.Current.Shutdown() );
            AboutCommand = new DelegateCommand( OnAbout );
        }

        public ICommand NewCommand { get; private set; }
        
        public ICommand OpenCommand { get; private set; }
        
        public ICommand CloseCommand { get; private set; }

        public ICommand AboutCommand { get; private set; }
    
        private void OnAbout()
        {
            Process.Start( "https://github.com/bg0jr/RaynMaker" );
        }
    }
}
