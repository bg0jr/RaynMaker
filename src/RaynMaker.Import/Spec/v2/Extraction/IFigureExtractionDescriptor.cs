namespace RaynMaker.Import.Spec.v2.Extraction
{
    public interface IFigureExtractionDescriptor
    {
        string Datum { get; set; }

        bool InMillions { get; set; }
    }
}
