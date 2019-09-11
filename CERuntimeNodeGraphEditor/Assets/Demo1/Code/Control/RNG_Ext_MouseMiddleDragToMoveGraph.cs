using CERuntimeNodeGraph.Code;
using UnityEngine;

namespace DefaultNamespace.Control
{
    public class RNG_Ext_MouseMiddleDragToMoveGraph : MonoBehaviour
    {
        private bool mIsDuringDrag;

        private Vector3 mViewDownPos;
        private Vector3 mMouseDownPos;

        private void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {
                mIsDuringDrag = true;
                mViewDownPos = RNG.Display.ContentRoot.position;
                mMouseDownPos = Input.mousePosition;
                return;
            }

            if (Input.GetMouseButtonUp(2))
            {
                mIsDuringDrag = false;
                return;
            }

            if (Input.GetMouseButton(2))
            {
                if (!mIsDuringDrag) return;
                RunMoveLogic();
            }
        }

        private void RunMoveLogic()
        {
            RNG.Display.ContentRoot.position = mViewDownPos + (Input.mousePosition - mMouseDownPos);
            RNG.Display.allNodeList.ForEach(node => node.MarkNeedRepaint());
        }
    }
}