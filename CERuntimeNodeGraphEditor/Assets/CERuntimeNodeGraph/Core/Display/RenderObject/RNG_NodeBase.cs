using UnityEngine;
using UnityEngine.EventSystems;

namespace CERuntimeNodeGraph.Code.GUI.RenderObject
{
    [RequireComponent(typeof(RectTransform))]
    public class RNG_NodeBase : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        protected RectTransform mRT;
        private bool mIsNodeSelected;

        private Rect mWorldRect;

        private void Start()
        {
            RNG.Display.allNodeList.Add(this);
            mRT = GetComponent<RectTransform>();
            RecalculateRect();
            Initialize();
        }

        protected virtual void Initialize() { }

        public Rect GetWorldRect() { return mWorldRect; }

        private void Update() { }

        /// <summary>
        /// 重新计算当前Rect区域,发生与Graph的移动或者Zoom.同时自身Move或者RT发生变化时候 也需要调用
        /// </summary>
        public void RecalculateRect()
        {
            var v = new Vector3[4];
            mRT.GetWorldCorners(v);
            mWorldRect = new Rect(v[0].x, v[0].y, v[3].x - v[0].x, v[1].y - v[0].y);
        }


        public bool IsNodeSelected
        {
            get { return mIsNodeSelected; }
            set
            {
                if (mIsNodeSelected == value) return;
                mIsNodeSelected = value;
                OnNodeSelectedChange(mIsNodeSelected);
            }
        }

        protected virtual void OnNodeSelectedChange(bool _isNowSelected) { }


        //==================
        //== Drag Drop
        //==================

        public void OnDrag(PointerEventData eventData) { RNG.Logic.Node.Move.OnDrag(eventData); }
        public void OnEndDrag(PointerEventData eventData) { RNG.Logic.Node.Move.OnEndDrag(eventData); }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //如果当前对象不是已经选择的对象,则取消之前的选择对象
            if (!IsNodeSelected)
            {
                RNG.Logic.Node.Select.CleanAllSelected();
            }

            IsNodeSelected = true;
            RNG.Logic.Node.Move.OnBeginDrag(eventData);
        }

        private Vector2 mStartMoveOffset;
        public void NodeOnBeginDragMove(PointerEventData eventData) { mStartMoveOffset = new Vector2(mRT.position.x, mRT.position.y) - eventData.position; }

        public void NodeOnEndDragMove(PointerEventData eventData) { }

        public void NodeOnDragMove(PointerEventData eventData) { mRT.position = eventData.position + mStartMoveOffset; }
    }
}