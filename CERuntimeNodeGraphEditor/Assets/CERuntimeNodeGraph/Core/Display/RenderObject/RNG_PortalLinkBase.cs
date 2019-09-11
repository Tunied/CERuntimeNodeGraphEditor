using System;
using Code.CopyEngine.Core.Notification;
using UnityEngine;

namespace CERuntimeNodeGraph.Code.GUI.RenderObject
{
    /// <summary>
    /// 连接的线条
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class RNG_PortalLinkBase : MonoBehaviour
    {
        public int UID;

        public int LinkLow;
        public int LinkHigh;

        private RectTransform mRT;

        private bool mIsNeedRepaint;

        public virtual void Initialize()
        {
            UID = CEUID.NewOne();
            mRT = GetComponent<RectTransform>();
        }

        /// <summary>
        /// 拖拽时候会空创建出来一个Link放在最上层用于显示,此时该对象不在allLinkList之中
        /// </summary>
        protected virtual void OnDestroy() { RNG.Display.allLinkList.Remove(this); }


        protected virtual void Update()
        {
            if (!mIsNeedRepaint) return;
            mIsNeedRepaint = false;

            var low = RNG.Logic.Portal.Serach.GetPortalByUID(LinkLow);
            var high = RNG.Logic.Portal.Serach.GetPortalByUID(LinkHigh);
            if (low == null || high == null)
            {
                Destroy(gameObject);
            }
            else
            {
                Render(low.GerJoinWorldPos(), high.GerJoinWorldPos());
                OnRepaint();
            }
        }
        
        protected virtual void OnRepaint() { }


        public virtual void MarkNeedRepaint() { mIsNeedRepaint = true; }

        public virtual void JoinPortal(RNG_PortalBase _from, RNG_PortalBase _to)
        {
            var min = Mathf.Min(_from.UID, _to.UID);
            var max = Mathf.Max(_from.UID, _to.UID);
            LinkLow = min;
            LinkHigh = max;
            mRT.transform.SetParent(RNG.Display.BottomLayerRoot, false);
            RNG.Display.allLinkList.Add(this);
            Render(_from.GerJoinWorldPos(), _to.GerJoinWorldPos());
        }

        public virtual void Render(Vector2 _wPosA, Vector2 _wPosB)
        {
            var mPA = new Vector2(_wPosA.x, _wPosA.y);
            var mPB = new Vector2(_wPosB.x, _wPosB.y);

            gameObject.SetActive(true);
            var dirV2 = mPB - mPA;
            var angle = Vector2.SignedAngle(dirV2, Vector2.down);

            mRT.position = _wPosA;
            mRT.localRotation = Quaternion.AngleAxis(-angle, Vector3.forward);

            var distance = Vector3.Distance(_wPosA, _wPosB);
            mRT.sizeDelta = new Vector2(3, Math.Max(1, distance) / RNG.Display.ContentRoot.localScale.x);
        }
    }
}