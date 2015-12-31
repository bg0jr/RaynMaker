namespace RaynMaker.Import.Spec.v2.Extraction
{
    /// <summary>
    /// Describes a figure within a document focusing on how it can be extracted.
    /// </summary>
    public interface IFigureDescriptor
    {
        string Figure { get; set; }

        bool InMillions { get; set; }
    }
}
