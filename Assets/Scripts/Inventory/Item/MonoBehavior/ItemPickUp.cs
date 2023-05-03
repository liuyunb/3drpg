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
            //��ӵ�����
            InventoryManager.Instance.bag.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.bagContainner.RefreshUI();
            //װ������
            //GameManager.Instance.playerData.EquipWeapon(itemData);
            //�������
            if (QuestManager.isInitialized)
                QuestManager.Instance.UpdateQuestProcess(itemData.itemName, 1);
            //����
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
