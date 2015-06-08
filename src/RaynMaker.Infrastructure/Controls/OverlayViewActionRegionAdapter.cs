using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Windows;
using Microsoft.Practices.Prism.Regions;

namespace RaynMaker.Infrastructure.Controls
{
    [Export( typeof( OverlayViewActionRegionAdapter ) )]
    public class OverlayViewActionRegionAdapter : RegionAdapterBase<OverlayViewAction>
    {
        [ImportingConstructor]
        public OverlayViewActionRegionAdapter( IRegionBehaviorFactory factory )
            : base( factory )
        {
        }

        protected override void Adapt( IRegion region, OverlayViewAction regionTarget )
        {
            region.Views.CollectionChanged += ( s, e ) =>
                {
                    if( e.Action == NotifyCollectionChangedAction.Add )
                    {
                        foreach( FrameworkElement element in e.NewItems )
                        {
                            regionTarget.ViewContent = element;
                        }
                    }
                };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
