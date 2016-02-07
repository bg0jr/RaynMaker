using RaynMaker.Modules.Import.Spec.v2;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    interface INotifyValidationFailed
    {
        void FailedToNavigateTo( DataSource dataSource, string error);
    }
}
