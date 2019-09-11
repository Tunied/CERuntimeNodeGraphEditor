using Code.CopyEngine.Core.Notification;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CERuntimeNodeGraph.Code.GUI.RenderObject
{
    public class RNG_PortalBase : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected RectTransform mRT;

        private bool mIsNodeSelected;

        private Rect mWorldRect;

        private Vector2 mDragDownPos;

        private bool mIsDirty;
        private bool mIsNeedLayout;
        private bool mIsNeedRepaint;

        //只用来在拖拽时候显示
        private RNG_PortalLinkBase mDuringDragLink;

        public int UID;

        protected virtual void Start()
        {
            UID = CEUID.NewOne();
            mRT = GetComponent<RectTransform>();
            Initialize();
        }

        public virtual void MarkNeedRepaint()
        {
            mIsDirty = true;
            mIsNeedLayout = true;
            RNG.Logic.PortalLink.Search.ForEachLinkContainID(UID, link => { link.MarkNeedRepaint(); });
        }


        protected virtual void Update()
        {
            if (!mIsDirty) return;
            mIsDirty = false;
            OnRepaint();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            mDragDownPos = eventData.pressPosition;
            mDuringDragLink = RNG.Assets.NewPortalLink(this);
            mDuringDragLink.transform.SetParent(RNG.Display.TopLaterRoot, false);
            RNG.Display.nowSelectPortal = this;
        }

        public void OnDrag(PointerEventData eventData) { mDuringDragLink.Render(GerJoinWorldPos(), eventData.position); }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (RNG.Display.nowWillLinkPortal != null)
            {
                RNG.Logic.Portal.Link.JoinPortal(this, RNG.Display.nowWillLinkPortal);
            }

            Destroy(mDuringDragLink.gameObject);
            RNG.Display.nowWillLinkPortal = null;
            RNG.Display.nowSelectPortal = null;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (RNG.Display.nowSelectPortal != null && RNG.Display.nowSelectPortal != this &&
                !RNG.Logic.Node.Search.IsTwoPortalUnderSameNode(RNG.Display.nowSelectPortal.UID, UID) &&
                !RNG.Logic.PortalLink.Search.IsExistLink(RNG.Display.nowSelectPortal.UID, UID) &&
                IsCanJoin(RNG.Display.nowSelectPortal))
            {
                OnCanJoinPortalLinkEnter(RNG.Display.nowSelectPortal);
                RNG.Display.nowWillLinkPortal = this;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (RNG.Display.nowWillLinkPortal == this)
            {
                OnCanJoinPortalLinkExit(RNG.Display.nowSelectPortal);
                RNG.Display.nowWillLinkPortal = null;
            }
        }


        protected virtual void OnRepaint() { }
        protected virtual void Initialize() { }

        protected virtual bool IsCanJoin(RNG_PortalBase _portal) { return true; }

        protected virtual void OnCanJoinPortalLinkEnter(RNG_PortalBase _portal) { }

        protected virtual void OnCanJoinPortalLinkExit(RNG_PortalBase _portal) { }

        public virtual void OnJoinPortal(RNG_PortalBase _portal) { }

        /// <summary>
        /// 当前连接点的World坐标,用于处理线PortalLink的连接
        /// </summary>
        public virtual Vector2 GerJoinWorldPos() { return new Vector2(mRT.position.x, mRT.position.y); }
    }
}