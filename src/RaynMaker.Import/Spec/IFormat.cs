namespace RaynMaker.Import.Spec
{
    public interface IFormat 
    {
        string Datum { get;  set; }
        
        IFormat Clone();
    }
}
