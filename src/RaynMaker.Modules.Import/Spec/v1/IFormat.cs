namespace RaynMaker.Modules.Import.Spec.v1
{
    interface IFormat
    {
        string Datum { get; set; }

        bool InMillions { get; set; }
    }
}
