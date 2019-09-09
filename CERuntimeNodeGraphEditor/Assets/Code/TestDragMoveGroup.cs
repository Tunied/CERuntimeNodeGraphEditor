using System;
using System.Collections.Generic;
using Code;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestDragMoveGroup : MonoBehaviour
{
    private const float ZOOM_DELTA = 0.1f;

    public static TestDragMoveGroup instance;

    public DragBaseObject[] allItems;

    private List<DragBaseObject> mNowSelectedItems;

    public Image selectArea;


    public Transform viewTrans;

    public Button moveBtn;
    public Button selectBtn;
    public Button zoomInBtn;
    public Button zoomOutBtn;

    private bool mIsNowSelectModel;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        mNowSelectedItems = new List<DragBaseObject>();

        zoomInBtn.onClick.AddListener(() =>
        {
            var s = viewTrans.localScale;
            s.x += ZOOM_DELTA;
            s.y = s.x;
            s.z = s.x;
            viewTrans.localScale = s;
        });

        zoomOutBtn.onClick.AddListener(() =>
        {
            var s = viewTrans.localScale;
            s.x -= ZOOM_DELTA;
            s.y = s.x;
            s.z = s.x;
            viewTrans.localScale = s;
        });

        moveBtn.onClick.AddListener(() =>
        {
            mIsNowSelectModel = false;
            Debug.Log("Move Model");
        });

        selectBtn.onClick.AddListener(() =>
        {
            mIsNowSelectModel = true;
            Debug.Log("Select Model");
        });
    }


    public void OnBeginDrag(DragBaseObject _target, PointerEventData eventData)
    {
        //点击要拖拽的Item不在所选范围内,清除原选择区域
        if (!mNowSelectedItems.Contains(_target))
        {
            mNowSelectedItems.Clear();
            mNowSelectedItems.Add(_target);
        }

        foreach (var item in mNowSelectedItems)
        {
            item.CEOnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        foreach (var item in mNowSelectedItems)
        {
            item.CEOnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (var item in mNowSelectedItems)
        {
            item.CEOnEndDrag(eventData);
        }

        mNowSelectedItems.Clear();
    }

    private Vector3 mDownPos;


    public void RectOnBeginDrag(Vector3 _wPosV3)
    {
        if (mIsNowSelectModel)
        {
            SelectOnBeginDrag(_wPosV3);
        }
        else
        {
            MoveOnBeginDrag(_wPosV3);
        }
    }

    public void RectOnDrag(Vector3 _wPosV3)
    {
        if (mIsNowSelectModel)
        {
            SelectOnDrag(_wPosV3);
        }
        else
        {
            MoveOnDrag(_wPosV3);
        }
    }

    public void RectOnEndDrag(Vector3 _wPosV3)
    {
        if (mIsNowSelectModel)
        {
            SelectOnEndDrag(_wPosV3);
        }
        else
        {
            MoveOnEndDrag(_wPosV3);
        }
    }


    private Vector3 mViewDownPos;
    private Vector3 mMouseDownPos;
    private Vector3 mMoveOffset;

    private void MoveOnBeginDrag(Vector3 _posV3)
    {
        Debug.Log("Start pos: " + _posV3);
        mViewDownPos = viewTrans.position;
        mMouseDownPos = _posV3;
    }

    private void MoveOnDrag(Vector3 _wPosV3) { viewTrans.position = mViewDownPos + (_wPosV3 - mMouseDownPos); }

    private void MoveOnEndDrag(Vector3 _wPosV3) { Debug.Log("End pos : " + _wPosV3); }


    private void SelectOnEndDrag(Vector3 _upPosV3)
    {
        selectArea.gameObject.SetActive(false);

        var minX = Mathf.Min(mDownPos.x, _upPosV3.x);
        var maxX = Mathf.Max(mDownPos.x, _upPosV3.x);
        var minY = Mathf.Min(mDownPos.y, _upPosV3.y);
        var maxY = Mathf.Max(mDownPos.y, _upPosV3.y);

        var width = Mathf.Max(maxX - minX, 1);
        var height = Mathf.Max(maxY - minY, 1);

        var rect = new Rect(minX, minY, width, height);
        mNowSelectedItems.Clear();

        DrawLine.instance.Test(rect);

        foreach (var item in allItems)
        {
            if (rect.Overlaps(item.Rect))
            {
                mNowSelectedItems.Add(item);
                Debug.Log($"Select item {item.name}");
            }
        }
    }

    private void SelectOnDrag(Vector3 _wPosV3) { UpdateSelectArea(mDownPos, _wPosV3); }


    private void SelectOnBeginDrag(Vector3 _downPosV3)
    {
        mNowSelectedItems.Clear();
        mDownPos = _downPosV3;
        selectArea.gameObject.SetActive(true);
    }

    private void UpdateSelectArea(Vector3 _posA, Vector3 _posB)
    {
        var minX = Mathf.Min(_posA.x, _posB.x);
        var maxX = Mathf.Max(_posA.x, _posB.x);
        var minY = Mathf.Min(_posA.y, _posB.y);
        var maxY = Mathf.Max(_posA.y, _posB.y);

        var width = Mathf.Max(maxX - minX, 1);
        var height = Mathf.Max(maxY - minY, 1);


        var wPos = new Vector3(minX, minY, 0);
//        var sPos = RectTransformUtility.WorldToScreenPoint(Camera.main, wPos);
        selectArea.rectTransform.position = wPos;
        selectArea.rectTransform.sizeDelta = new Vector2(width, height);
    }
}