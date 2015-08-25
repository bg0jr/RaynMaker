using Microsoft.Practices.Prism.PubSubEvents;
using RaynMaker.Entities;

namespace RaynMaker.Infrastructure.Events
{
    public class AssetSelectedEvent : PubSubEvent<Stock>
    {
    }
}
