
namespace RaynMaker.Infrastructure.Mvvm
{
    /// <summary>
    /// Implemented by ViewModels to allow views to trigger initial validation at the right point in time.
    /// E.g. if the view works with Validation.ErrorTemplate (which uses Adorners) the initial validation of the 
    /// ViewModel properties can only be done after the view is loaded otherwise the adorners - and so the error template - will
    /// not be show.
    /// </summary>
    public interface IValidationAware
    {
        bool ValidateProperties();
    }
}
