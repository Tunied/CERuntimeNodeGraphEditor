using System.Linq;
using CERuntimeNodeGraph.Code;
using UnityEngine;

namespace DefaultNamespace
{
    public class RNG_NodeSearchLogic
    {
        public bool IsTwoPortalUnderSameNode(int _aID, int _bID)
        {
            if (_aID == _bID)
            {
                Debug.LogError("8kMaJnE4");
                return true;
            }

            return RNG.Display.allNodeList.Exists(node =>
            {
                var count = node.subPortals.Count(portal => portal.UID == _aID || portal.UID == _bID);
                return count == 2;
            });
        }
    }
}