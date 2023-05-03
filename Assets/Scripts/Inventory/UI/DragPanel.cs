using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour,IDragHandler,IPointerDownHandler
{
    RectTransform rectTransform;
    Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

    }
    private void Start()
    {
        canvas = InventoryManager.Instance.GetComponent<Canvas>();
    }

    private void Update()
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        print(canvas);
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        print(11);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.SetSiblingIndex(2);
    }
}
