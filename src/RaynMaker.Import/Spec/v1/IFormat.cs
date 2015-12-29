namespace RaynMaker.Import.Spec.v1
{
    public interface IFormat
    {
        string Datum { get; set; }

        bool InMillions { get; set; }
    }
}
