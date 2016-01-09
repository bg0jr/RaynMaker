using RaynMaker.Modules.Import.Spec.v2.Extraction;

namespace RaynMaker.Modules.Import.Web.ViewModels
{
    interface IDescriptorViewModel
    {
        IFigureDescriptor Descriptor { get; }

        IDocument Document { set; }

        void Unmark();
    }
}
