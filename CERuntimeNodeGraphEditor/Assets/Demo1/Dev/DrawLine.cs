using System;
using UnityEngine.UI;

namespace UnityEngine
{
    public class DrawLine : UnityEngine.MonoBehaviour
    {
        public float width = 5f;
        public RectTransform root;
        public static DrawLine instance;

        private Image mImg;

        private Vector2 mPA;
        private Vector2 mPB;

        private void Start()
        {
            mImg = GetComponent<Image>();
            instance = this;
            gameObject.SetActive(false);
        }


        public void Draw(Vector3 _wPosA, Vector3 _wPosB)
        {
            mPA = new Vector2(_wPosA.x, _wPosA.y);
            mPB = new Vector2(_wPosB.x, _wPosB.y);

            gameObject.SetActive(true);
            var dirV2 = mPB - mPA;
            var angle = Vector2.SignedAngle(dirV2, Vector2.down);

            mImg.transform.position = _wPosA;
            mImg.transform.localRotation = Quaternion.AngleAxis(-angle, Vector3.forward);

            var s = 1f / root.localScale.x;
            var distance = Vector3.Distance(_wPosA, _wPosB);
            mImg.rectTransform.sizeDelta = new Vector2(width, Math.Max(1, distance) * s);
        }

        public void Test(Rect _r)
        {
            if (Math2D.Intersects(mPA, mPB, _r))
            {
                gameObject.SetActive(false);
            }
        }


        public void Hide()
        {
//            gameObject.SetActive(false);
        }
    }
}