using CERuntimeNodeGraph.Code.GUI.RenderObject;

namespace CERuntimeNodeGraph.Code
{
    public interface RNG_IAssetsFactory
    {
        RNG_PortalLinkBase NewPortalLink(RNG_PortalBase _startPortal);
    }
}