using CERuntimeNodeGraph.Code;
using UnityEngine;

namespace DefaultNamespace
{
    public class RNG_NodeRectSelectLogic
    {
        public void SelectNodesInRect(Rect _worldRect, bool _isCleanBefore)
        {
            RNG.Display.allNodeList.ForEach(node =>
            {
                var isNowSelected = node.GetWorldRect().Overlaps(_worldRect);
                if (isNowSelected)
                {
                    node.IsNodeSelected = isNowSelected;
                }
                else
                {
                    if (_isCleanBefore && node.IsNodeSelected)
                    {
                        node.IsNodeSelected = false;
                    }
                }
            });
        }

        public void CleanAllSelected() { RNG.Display.allNodeList.ForEach(node => { node.IsNodeSelected = false; }); }
    }
}