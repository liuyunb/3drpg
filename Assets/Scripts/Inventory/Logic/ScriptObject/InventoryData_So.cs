using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Data",menuName = "Inventory/Inventory Data")]
public class InventoryData_So : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem> ();

    public void AddItem(ItemData_SO itemdata,int amount)
    {
        bool found = false;
        
        if(itemdata.stackable)
        {

            foreach (InventoryItem item in items)
            {
                if(item.ItemData == itemdata)
                {
                    found = true;
                    item.amount += amount;
                    break;
                }
            }
        }

        if(!found)
        {
            for(int i = 0;i < items.Count;i++)
            {
                if(items[i].ItemData == null)
                {
                    items[i].ItemData = itemdata;
                    items[i].amount = amount;
                    items[i].index = i;
                    break;
                }
            }
        }

    }
}
[System.Serializable]
public class InventoryItem
{
    public ItemData_SO ItemData;

    public int amount;

    public int index;
}
