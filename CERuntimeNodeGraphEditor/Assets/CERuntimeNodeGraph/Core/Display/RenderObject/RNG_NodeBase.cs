using System;
using Code.CopyEngine.Core.Notification;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CERuntimeNodeGraph.Code.GUI.RenderObject
{
    [RequireComponent(typeof(RectTransform))]
    public class RNG_NodeBase : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        protected RectTransform mRT;

        private bool mIsNodeSelected;
        private Vector2 mStartMoveOffset;
        private Rect mWorldRect;

        private bool mIsNeedRepaint;

        public int UID;

        /// <summary>
        /// 当前节点下的所有Portal
        /// </summary>
        [HideInInspector]
        public RNG_PortalBase[] subPortals;

        private void Start()
        {
            UID = CEUID.NewOne();
            mRT = GetComponent<RectTransform>();
            subPortals = GetComponentsInChildren<RNG_PortalBase>();
            RNG.Display.allNodeList.Add(this);
            RecalculateRect();
            Initialize();
        }

        public void MarkNeedRepaint()
        {
            mIsNeedRepaint = true;
            foreach (var portal in subPortals)
            {
                portal.MarkNeedRepaint();
            }
        }


        private void OnDestroy()
        {
            RNG.Display.allNodeList.Remove(this);
            foreach (var portal in subPortals)
            {
                portal.MarkNeedRepaint();
            }
        }


        private void Update()
        {
            if (!mIsNeedRepaint) return;
            mIsNeedRepaint = false;
            OnRepaint();
            RecalculateRect();
        }

        public Rect GetWorldRect() { return mWorldRect; }


        /// <summary>
        /// 重新计算当前Rect区域,发生与Graph的移动或者Zoom.同时自身Move或者RT发生变化时候 也需要调用
        /// </summary>
        private void RecalculateRect()
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


        //====================
        //== 可重载函数
        //====================

        protected virtual void Initialize() { }

        protected virtual void OnRepaint() { }

        protected virtual void OnNodeSelectedChange(bool _isNowSelected) { }

        public virtual void NodeOnBeginDragMove(PointerEventData eventData) { mStartMoveOffset = new Vector2(mRT.position.x, mRT.position.y) - eventData.position; }

        public virtual void NodeOnEndDragMove(PointerEventData eventData) { }

        public virtual void NodeOnDragMove(PointerEventData eventData)
        {
            mRT.position = eventData.position + mStartMoveOffset;
            MarkNeedRepaint();
        }
    }
}