using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //添加到背包
            InventoryManager.Instance.bag.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.bagContainner.RefreshUI();
            //装备武器
            //GameManager.Instance.playerData.EquipWeapon(itemData);
            //完成任务
            if (QuestManager.isInitialized)
                QuestManager.Instance.UpdateQuestProcess(itemData.itemName, 1);
            //销毁
            Destroy(gameObject);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {

    //    }
    //}
}
