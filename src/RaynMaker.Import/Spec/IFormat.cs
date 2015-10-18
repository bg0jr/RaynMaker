namespace RaynMaker.Import.Spec
{
    public interface IFormat
    {
        string Datum { get; set; }

        bool InMillions { get; set; }

        IFormat Clone();
    }
}
