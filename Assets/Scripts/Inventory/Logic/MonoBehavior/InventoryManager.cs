using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : SingleTon<InventoryManager>
{
    public class DragData
    {
        public SlotHolder orginSlot;
        public RectTransform orginParent;
    }

    [Header("Inventory Info")]
    public InventoryData_So bag;
    public InventoryData_So bagTemplate;
    public InventoryData_So action;
    public InventoryData_So actionTemplate;
    public InventoryData_So statas;
    public InventoryData_So statasTemplate;


    [Header("Containner")]
    public ContainnerUI bagContainner;
    public ContainnerUI actionContainner;
    public ContainnerUI statasContainner;

    public Canvas dragCanvas;
    public DragData dragItemData;

    public GameObject bagPanel;
    public GameObject statasPanel;

    [Header("Tooltip")]
    public ToolTip toolTip;
    [Header("Statas Text")]
    public Text health;
    public Text attack;
    
    bool getTab;
    bool isOpen;

    protected override void Awake()
    {
        base.Awake();
        bag = Instantiate(bagTemplate);
        action = Instantiate(actionTemplate);
        statas = Instantiate(statasTemplate);
    }

    private void Start()
    {
        LoadData();
        bagContainner.RefreshUI();
        actionContainner.RefreshUI();
        statasContainner.RefreshUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            getTab = true;
        }
    }

    private void FixedUpdate()
    {
        if(getTab)
        {
            getTab = false;
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statasPanel.SetActive(isOpen);
            bagContainner.RefreshUI();
            statasContainner.RefreshUI();

            if(!isOpen)
            {
                toolTip.gameObject.SetActive(false);
                print("areyousure");
            }
        }
    }

    public bool CheckInBagSlot(Vector3 position)
    {

        for(int i = 0;i < bagContainner.slotHolders.Length; i++)
        {
            RectTransform temp = bagContainner.slotHolders[i].transform as RectTransform;
            if(RectTransformUtility.RectangleContainsScreenPoint(temp,position))
            {
                return true;
            }
        }

        return false;
    }
    public bool CheckInActionSlot(Vector3 position)
    {

        for (int i = 0; i < actionContainner.slotHolders.Length; i++)
        {
            RectTransform temp = actionContainner.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(temp, position))
            {
                return true;
            }
        }

        return false;
    }
    public bool CheckInStatasSlot(Vector3 position)
    {

        for (int i = 0; i < statasContainner.slotHolders.Length; i++)
        {
            RectTransform temp = statasContainner.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(temp, position))
            {
                return true;
            }
        }

        return false;
    }

    public void UpdateStatasText(int maxHealth,int minDamage, int maxDamage)
    {
        health.text = maxHealth.ToString();
        attack.text = minDamage + " - " + maxDamage;
    }

    public void SaveData()
    {
        SaveManager.Instance.Save(bag, bag.name);
        SaveManager.Instance.Save(action, action.name);
        SaveManager.Instance.Save(statas, statas.name);
    }

    public void LoadData()
    {
        SaveManager.Instance.Load(bag, bag.name);
        SaveManager.Instance.Load(action, action.name);
        SaveManager.Instance.Load(statas, statas.name);
    }

    public void CheckQuestItemInBag(string requireName)
    {
        foreach (var item in bag.items)
        {
            if (item.ItemData != null && item.ItemData.itemName == requireName)
                QuestManager.Instance.UpdateQuestProcess(requireName, item.amount);
        }
        foreach (var item in action.items)
        {
            if (item.ItemData != null && item.ItemData.itemName == requireName)
                QuestManager.Instance.UpdateQuestProcess(requireName, item.amount);
        }
    }

    public InventoryItem CheckItemInBag(ItemData_SO itemData)
    {
        foreach (var item in bag.items)
        {
            if(item.ItemData != null && item.ItemData == itemData)
                return item;
        }
        return null;
    }
    public InventoryItem CheckItemInAction(ItemData_SO itemData)
    {
        foreach (var item in action.items)
        {
            if (item.ItemData != null && item.ItemData == itemData)
                return item;
        }
        return null;
    }
}
