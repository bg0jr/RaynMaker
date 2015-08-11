
namespace RaynMaker.Blade.DataSheetSpec
{
    public interface IFreezable
    {
        bool IsFrozen { get; }
        void Freeze();
    }
}
