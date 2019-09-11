using System;
using CERuntimeNodeGraph.Code;
using CERuntimeNodeGraph.Code.GUI.RenderObject;
using UnityEngine;

namespace DefaultNamespace
{
    public class RNG_PortalLinkSearchLogic
    {
        /// <summary>
        /// 是否已经存在Link
        /// </summary>
        public bool IsExistLink(int _aID, int _bID)
        {
            var low = Mathf.Min(_aID, _bID);
            var high = Mathf.Max(_aID, _bID);
            return RNG.Display.allLinkList.Exists(link => link.LinkLow == low && link.LinkHigh == high);
        }

        public void ForEachLinkContainID(int _portalID, Action<RNG_PortalLinkBase> _callback)
        {
            RNG.Display.allLinkList.ForEach(link =>
            {
                if (link.LinkLow == _portalID || link.LinkHigh == _portalID)
                {
                    _callback(link);
                }
            });
        }
    }
}