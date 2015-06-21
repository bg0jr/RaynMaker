using RaynMaker.Blade.DataSheetSpec;

namespace RaynMaker.Blade.Engine
{
    public interface IReportElement
    {
        void Report( ReportContext context );
    }
}
