using Prism.Mvvm;
using Plainion;
using RaynMaker.Modules.Import.Web.Model;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    class SpecDefinitionViewModelBase : BindableBase
    {
        protected SpecDefinitionViewModelBase( Session session )
        {
            Contract.RequiresNotNull( session, "session" );

            Session = session;
        }

        public Session Session { get; private set; }
    }
}
