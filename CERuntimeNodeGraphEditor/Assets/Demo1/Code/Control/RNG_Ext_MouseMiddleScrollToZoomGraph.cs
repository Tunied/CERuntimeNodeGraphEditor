using CERuntimeNodeGraph.Code;
using UnityEngine;

namespace DefaultNamespace.Control
{
    public class RNG_Ext_MouseMiddleScrollToZoomGraph : MonoBehaviour
    {
        public float minZoomSize = 0.3f;
        public float maxZoomSize = 2f;
        public float scrollSpeed = 0.1f;

        public Transform Graph;


        private void Update()
        {
            var dir = Input.mouseScrollDelta.y;
            if (dir == 0) return;

            var nowScale = Graph.localScale;

            nowScale.x += dir > 0 ? scrollSpeed : -scrollSpeed;
            nowScale.x = Mathf.Min(nowScale.x, maxZoomSize);
            nowScale.x = Mathf.Max(nowScale.x, minZoomSize);

            nowScale.y = nowScale.x;
            nowScale.z = nowScale.x;

            Graph.localScale = nowScale;

            RNG.Display.allNodeList.ForEach(node => node.RecalculateRect());
        }
    }
}