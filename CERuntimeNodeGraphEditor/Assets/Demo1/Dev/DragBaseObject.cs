using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code
{
    public class DragBaseObject : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        //存储图片中心点与鼠标点击点的偏移量
        private Vector3 mOffset;

        //存储当前拖拽图片的RectTransform组件
        private RectTransform mRT;

        private void Start() { mRT = gameObject.GetComponent<RectTransform>(); }

        public void OnBeginDrag(PointerEventData eventData) { TestDragMoveGroup.instance.OnBeginDrag(this, eventData); }

        public void OnDrag(PointerEventData eventData) { TestDragMoveGroup.instance.OnDrag(eventData); }

        public void OnEndDrag(PointerEventData eventData) { TestDragMoveGroup.instance.OnEndDrag(eventData); }


        public void CEOnBeginDrag(PointerEventData eventData)
        {
            // 存储点击时的鼠标坐标
            Vector3 tWorldPos;
            //UI屏幕坐标转换为世界坐标
            RectTransformUtility.ScreenPointToWorldPointInRectangle(mRT, eventData.position, eventData.pressEventCamera, out tWorldPos);
            //计算偏移量   
            mOffset = transform.position - tWorldPos;
        }

        public void CEOnDrag(PointerEventData eventData) { SetDraggedPosition(eventData); }

        public void CEOnEndDrag(PointerEventData eventData) { SetDraggedPosition(eventData); }

        public Rect Rect
        {
            get
            {
                var v = new Vector3[4];
                mRT.GetWorldCorners(v);
                return new Rect(v[0].x, v[0].y, v[3].x - v[0].x, v[1].y - v[0].y);
            }
        }

        /// <summary>
        /// 设置图片位置方法
        /// </summary>
        /// <param name="eventData"></param>
        private void SetDraggedPosition(PointerEventData eventData)
        {
            //存储当前鼠标所在位置
            Vector3 globalMousePos;
            //UI屏幕坐标转换为世界坐标
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(mRT, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                //设置位置及偏移量
                mRT.position = globalMousePos + mOffset;
            }
        }
    }
}