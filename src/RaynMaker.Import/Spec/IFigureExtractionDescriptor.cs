namespace RaynMaker.Import.Spec
{
    public interface IFigureExtractionDescriptor
    {
        string Datum { get; set; }

        bool InMillions { get; set; }
    }
}
