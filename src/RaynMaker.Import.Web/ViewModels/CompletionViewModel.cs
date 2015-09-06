using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using RaynMaker.Import.Web.Model;

namespace RaynMaker.Import.Web.ViewModels
{
    class CompletionViewModel
    {
        private Session mySession;

        public CompletionViewModel( Model.Session session )
        {
            mySession = session;

            ClearCommand = new DelegateCommand( OnClear );
            SaveCommand = new DelegateCommand( OnSave );
        }

        public ICommand ClearCommand { get; private set; }

        private void OnClear()
        {
            mySession.CurrentSite = null;
            mySession.CurrentLocator = null;
        }

        public ICommand SaveCommand { get; private set; }

        private void OnSave()
        {

        }
    }
}
