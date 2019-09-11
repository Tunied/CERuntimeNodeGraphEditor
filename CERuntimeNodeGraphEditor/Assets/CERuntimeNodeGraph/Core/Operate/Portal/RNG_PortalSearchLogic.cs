using System.Linq;
using CERuntimeNodeGraph.Code;
using CERuntimeNodeGraph.Code.GUI.RenderObject;

namespace DefaultNamespace
{
    public class RNG_PortalSearchLogic
    {
        public RNG_PortalBase GetPortalByUID(int _uid)
        {
            return RNG.Display.allNodeList.SelectMany(node => node.subPortals).FirstOrDefault(portal => portal.UID == _uid);
        }
    }
}