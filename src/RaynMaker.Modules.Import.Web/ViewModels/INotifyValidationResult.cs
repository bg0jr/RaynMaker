using RaynMaker.Modules.Import.Spec.v2;
using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    interface INotifyValidationFailed
    {
        void FailedToLocateDocument( DataSource dataSource, string error);

        void NavigationSucceeded( DataSource dataSource );

        void FailedToParseDocument( IFigureDescriptor figureDescriptor, string error );

        void ParsingSucceeded( IFigureDescriptor figureDescriptor );
    }
}
