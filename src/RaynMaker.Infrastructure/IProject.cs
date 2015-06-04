using Plainion.AppFw.Wpf.Infrastructure;
using RaynMaker.Entities;

namespace RaynMaker.Infrastructure
{
    public interface IProject
    {
        IAssetsContext CreateAssetsContext();
    }
}
