using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.AppFw.Wpf.Infrastructure;
using RaynMaker.Infrastructure;

namespace RaynMaker.Browser
{
    [Export]
    class BrowserViewModel : BindableBase
    {
        private IProjectHost myProjectHost;

        [ImportingConstructor]
        public BrowserViewModel( IProjectHost host )
        {
            myProjectHost = host;

            myProjectHost.Changed += OnProjectChanged;
        }

        private void OnProjectChanged()
        {
            OnPropertyChanged( () => HasProject );
        }

        public bool HasProject { get { return myProjectHost.Project != null; } }
    }
}
