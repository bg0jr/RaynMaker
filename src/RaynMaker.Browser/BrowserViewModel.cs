using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Mvvm;
using Plainion.AppFw.Wpf.Infrastructure;
using RaynMaker.Infrastructure;

namespace RaynMaker.Browser
{
    [Export]
    class BrowserViewModel : BindableBase
    {
        private IProjectService<Project> myProjectService;

        [ImportingConstructor]
        public BrowserViewModel( IProjectService<Project> projectService )
        {
            myProjectService = projectService;

            myProjectService.ProjectChanged += OnProjectChanged;
        }

        private void OnProjectChanged( ProjectBase obj )
        {
            OnPropertyChanged( () => HasProject );
        }

        public bool HasProject { get { return myProjectService.Project != null; } }
    }
}
