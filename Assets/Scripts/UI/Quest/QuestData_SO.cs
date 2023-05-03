using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "QuestData",menuName = "Dialog/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire
    {
        public string requireName;
        public int requireNum;
        public int currentNum;
    }

    public string questName;

    [TextArea]
    public string questDescription;

    public bool isStarted;
    public bool isCompleted;
    public bool isFinished;

    public List<QuestRequire> requireLists = new List<QuestRequire>();
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public void CheckQuestProcess()
    {
        var finishedQuest = requireLists.Where(r => r.requireNum <= r.currentNum);
        isCompleted = finishedQuest.Count() == requireLists.Count();
    }

    public void GiveRewards()
    {
        foreach (var item in inventoryItems)
        {
            if (item.amount < 0)
            {
                int requireNum = Mathf.Abs(item.amount);
                if (InventoryManager.Instance.CheckItemInBag(item.ItemData) != null)
                {
                    if (InventoryManager.Instance.CheckItemInBag(item.ItemData).amount < requireNum)
                    {
                        requireNum -= InventoryManager.Instance.CheckItemInBag(item.ItemData).amount;
                        InventoryManager.Instance.CheckItemInBag(item.ItemData).amount = 0;

                        InventoryManager.Instance.CheckItemInAction(item.ItemData).amount -= requireNum;
                    }
                    else
                        InventoryManager.Instance.CheckItemInBag(item.ItemData).amount -= requireNum;


                }
                else
                    InventoryManager.Instance.CheckItemInAction(item.ItemData).amount -= requireNum;
            }
            else
                InventoryManager.Instance.bag.AddItem(item.ItemData,item.amount);

            InventoryManager.Instance.bagContainner.RefreshUI();
            InventoryManager.Instance.actionContainner.RefreshUI();
        }
    }

}
