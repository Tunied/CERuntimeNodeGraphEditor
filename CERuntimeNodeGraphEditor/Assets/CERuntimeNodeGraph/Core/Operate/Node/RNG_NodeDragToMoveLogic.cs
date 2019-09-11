using CERuntimeNodeGraph.Code;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class RNG_NodeDragToMoveLogic
    {
        public void OnDrag(PointerEventData eventData)
        {
            RNG.Display.allNodeList.ForEach(node =>
            {
                if (node.IsNodeSelected)
                {
                    node.NodeOnDragMove(eventData);
                }
            });
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RNG.Display.allNodeList.ForEach(node =>
            {
                if (node.IsNodeSelected)
                {
                    node.NodeOnEndDragMove(eventData);
                    node.RecalculateRect();
                }
            });
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RNG.Display.allNodeList.ForEach(node =>
            {
                if (node.IsNodeSelected)
                {
                    node.NodeOnBeginDragMove(eventData);
                }
            });
        }
    }
}