using CERuntimeNodeGraph.Code;
using CERuntimeNodeGraph.Code.GUI.RenderObject;

namespace DefaultNamespace
{
    public class RNG_PortalDragToLinkLogic
    {
        public void JoinPortal(RNG_PortalBase _from, RNG_PortalBase _to)
        {
            _from.OnJoinPortal(_to);
            _to.OnJoinPortal(_from);

            var link = RNG.Assets.NewPortalLink(_from);
            link.JoinPortal(_from, _to);
        }
    }
}