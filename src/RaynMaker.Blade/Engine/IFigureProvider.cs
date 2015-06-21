using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    interface IFigureProvider
    {
        string Name { get; }

        object ProvideValue( Asset asset );
    }
}
