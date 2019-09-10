using CERuntimeNodeGraph.Code.GUI.RenderObject;

namespace CERuntimeNodeGraph.Code
{
    public abstract class RNG_AssetsFactoryBase
    {
        public abstract RNG_PortalLinkBase NewPortalLink(RNG_PortalBase _startPortal);
    }
}