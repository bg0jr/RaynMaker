using Prism.Events;

namespace RaynMaker.Infrastructure.Events
{
    /// <summary>
    /// Publishes the GUID of the deleted company.
    /// </summary>
    public class CompanyDeletedEvent : PubSubEvent<string>
    {
    }
}
