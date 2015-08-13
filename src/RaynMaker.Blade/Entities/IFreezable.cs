
namespace RaynMaker.Blade.Entities
{
    public interface IFreezable
    {
        bool IsFrozen { get; }
        void Freeze();
    }
}
