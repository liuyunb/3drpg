using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItem;
    RectTransform itemRectTransform;
    SlotHolder currentSlot;
    SlotHolder targetSlot;
    Vector2 minOffect;
    Vector2 maxOffect;

    private void Awake()
    {
        currentItem = GetComponent<ItemUI>();
        currentSlot = GetComponentInParent<SlotHolder>();
        itemRectTransform = transform as RectTransform;
    }
    private void Start()
    {
        minOffect = itemRectTransform.offsetMin;
        maxOffect = itemRectTransform.offsetMax;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //保存初始数据
        InventoryManager.Instance.dragItemData = new InventoryManager.DragData();
        InventoryManager.Instance.dragItemData.orginSlot = currentSlot;
        InventoryManager.Instance.dragItemData.orginParent = (RectTransform)transform.parent;
        //换一个层级
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            //判断是否在三个bag中slot上
            if(InventoryManager.Instance.CheckInBagSlot(eventData.position) || InventoryManager.Instance.CheckInActionSlot(eventData.position) || InventoryManager.Instance.CheckInStatasSlot(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetSlot = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                else
                    targetSlot = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                if(targetSlot != currentSlot)
                    switch (targetSlot.slotType)
                    {
                        case SlotType.Bag:
                            SwapBagItem();
                            break;
                        case SlotType.Weapon:
                            if (currentItem.Data.items[currentItem.index].ItemData.itemType == ItemType.Weapon)
                                SwapBagItem();
                            break;
                        case SlotType.Armor:
                            if (currentItem.Data.items[currentItem.index].ItemData.itemType == ItemType.Armor)
                                SwapBagItem();
                            break;
                        case SlotType.Action:
                            if (currentItem.Data.items[currentItem.index].ItemData.itemType == ItemType.Usable)
                                SwapBagItem();
                            break;
                        default:
                            break;
                    }

                currentSlot.UpdateUI();
                targetSlot.UpdateUI();
            }
        }
        transform.SetParent(InventoryManager.Instance.dragItemData.orginParent);
        itemRectTransform.offsetMin = minOffect;
        itemRectTransform.offsetMax = maxOffect;
    }

    public void SwapBagItem()
    { 
        var tempItem = currentSlot.itemUI.Data.items[currentSlot.itemUI.index];
        var targetItem = targetSlot.itemUI.Data.items[targetSlot.itemUI.index];

        bool isSameItem = tempItem.ItemData == targetItem.ItemData;


        if(isSameItem && targetItem.ItemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.ItemData = null;
            tempItem.amount = 0;
        }
        else
        {
            targetSlot.itemUI.Data.items[targetSlot.itemUI.index]= tempItem;
            currentSlot.itemUI.Data.items[currentSlot.itemUI.index] = targetItem;
            print("0.0");
        }

    }
}
