using UnityEngine;
using UnityEngine.EventSystems;

namespace Code
{
    public class TestLink : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        private Vector3 mStartPos;

        //存储当前拖拽图片的RectTransform组件
        private RectTransform mRT;
        private Camera cam;

        private void Start()
        {
            mRT = gameObject.GetComponent<RectTransform>();
            cam = Camera.main;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // 存储点击时的鼠标坐标
            Vector3 tWorldPos;
            //UI屏幕坐标转换为世界坐标
            RectTransformUtility.ScreenPointToWorldPointInRectangle(mRT, eventData.position, eventData.pressEventCamera, out tWorldPos);

            DrawLine.instance.Draw(mRT.position, tWorldPos);
        }

        public void OnEndDrag(PointerEventData eventData) { DrawLine.instance.Hide(); }
        public void OnBeginDrag(PointerEventData eventData) { }
    }
}