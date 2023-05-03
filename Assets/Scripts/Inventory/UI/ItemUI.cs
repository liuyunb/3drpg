using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image icon;
    
    public Text amountText;
    //��ʾ�ĸ����ݿ�
    public InventoryData_So Data { get; set; }

    public ItemData_SO itemData;
    //���ݿ��еĵڼ���
    public int index { get; set; } = -1;

    public void SetUpItemUI(ItemData_SO itemData,int amount)
    {
        if(amount <= 0)
        {
            this.itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }

        if(itemData != null)
        {
            this.itemData = itemData;
            icon.sprite = itemData.itemIcon;
            amountText.text = amount.ToString();
            icon.gameObject.SetActive(true);
        }
        else
            icon.gameObject.SetActive(false);

    }
    public InventoryItem GetItem()
    {

        return Data?.items[index];
    }
}

