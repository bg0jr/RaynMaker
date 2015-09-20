using Microsoft.Practices.Prism.Mvvm;
using Plainion;
using RaynMaker.Import.Web.Model;

namespace RaynMaker.Import.Web.ViewModels
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
