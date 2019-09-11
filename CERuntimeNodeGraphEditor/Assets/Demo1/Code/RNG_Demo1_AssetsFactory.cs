using CERuntimeNodeGraph.Code;
using CERuntimeNodeGraph.Code.GUI.RenderObject;
using UnityEngine;

namespace DefaultNamespace
{
    public class RNG_Demo1_AssetsFactory : MonoBehaviour, RNG_IAssetsFactory
    {
        public GameObject PortalLinkPrefab;

        public RNG_PortalLinkBase NewPortalLink(RNG_PortalBase _startPortal)
        {
            var go = Instantiate(PortalLinkPrefab);
            var sp = go.GetComponent<RNG_PortalLinkBase>();
            sp.Initialize();
            return sp;
        }
    }
}