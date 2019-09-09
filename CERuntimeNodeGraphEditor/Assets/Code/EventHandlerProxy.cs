using UnityEngine;
using UnityEngine.EventSystems;

namespace Code
{
    public class EventHandlerProxy : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        private RectTransform mRT;
        private void Start() { mRT = gameObject.GetComponent<RectTransform>(); }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 tWorldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(mRT, eventData.position, eventData.pressEventCamera, out tWorldPos);
            TestDragMoveGroup.instance.RectOnDrag(tWorldPos);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 tWorldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(mRT, eventData.position, eventData.pressEventCamera, out tWorldPos);
            TestDragMoveGroup.instance.RectOnEndDrag(tWorldPos);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Vector3 tWorldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(mRT, eventData.position, eventData.pressEventCamera, out tWorldPos);
            TestDragMoveGroup.instance.RectOnBeginDrag(tWorldPos);
        }
    }
}