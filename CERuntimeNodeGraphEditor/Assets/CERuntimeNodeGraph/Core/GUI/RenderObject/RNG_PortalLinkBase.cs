using DefaultNamespace;
using UnityEngine;

namespace CERuntimeNodeGraph.Code.GUI.RenderObject
{
    /// <summary>
    /// 连接的线条
    /// </summary>
    public class RNG_PortalLinkBase : MonoBehaviour
    {
        protected RNG_DataPortalLink mData;
        public virtual void SetData(RNG_DataPortalLink _data) { mData = _data; }


        public void Render(Vector2 _wPosA, Vector2 _wPosB) { }
    }
}