using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType { Bag, Weapon, Armor, Action}
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;

    public ItemUI itemUI;



    public void UpdateUI()
    {
        switch (slotType)
        {
            case SlotType.Bag:
                itemUI.Data = InventoryManager.Instance.bag;
                break;
            case SlotType.Weapon:
                itemUI.Data = InventoryManager.Instance.statas;
                    if (itemUI.Data.items[itemUI.index].ItemData != null)
                        GameManager.Instance.playerData?.ChangeWeapon(itemUI.Data.items[itemUI.index].ItemData);
                    else
                        GameManager.Instance.playerData?.UnEquipWeapon();
                break;
            case SlotType.Armor:
                //itemUI.Data = InventoryManager.Instance.bag;
                break;
            case SlotType.Action:
                itemUI.Data = InventoryManager.Instance.action;
                break;
        }
        var item = itemUI.Data?.items[itemUI.index];

        if(item != null)
        {
            if (item.amount <= 0)
            {
                item.ItemData = null;
                item.amount = 0;
            }

            if (item.ItemData == null)
                itemUI.SetUpItemUI(item.ItemData, 0);
            else
                itemUI.SetUpItemUI(item.ItemData, item.amount);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if(itemUI.GetItem()?.ItemData != null)
        {
            if (itemUI.GetItem().ItemData.itemType == ItemType.Usable && itemUI.GetItem().amount > 0)
            {
                GameManager.Instance.playerData.ApplyUsableItem(itemUI.GetItem().ItemData.usableData);
                itemUI.GetItem().amount--;
                if (QuestManager.isInitialized)
                    QuestManager.Instance.UpdateQuestProcess(itemUI.itemData.itemName, -1);
            }
        }
        UpdateUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemUI != null && itemUI.GetItem()?.ItemData != null)
        {
            InventoryManager.Instance.toolTip.gameObject.SetActive(true);
            InventoryManager.Instance.toolTip.SetUpToolTip(itemUI.GetItem().ItemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }
}
