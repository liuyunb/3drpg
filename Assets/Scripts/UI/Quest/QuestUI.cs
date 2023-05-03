using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : SingleTon<QuestUI>
{
    [Header("Quest")]
    public GameObject questPanel;
    [Header("ToolTip")]
    public ToolTip toolTip;
    bool isOpen;

    [Header("QuestList")]
    public RectTransform questList;
    public QuestButton questButton;

    [Header("Require")]
    public RectTransform requireList;
    public QuestRequire questRequire;

    [Header("Content")]
    public Text content;

    [Header("Reward")]
    public RectTransform rewardList;
    public ItemUI questReward;

    bool getQ;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            getQ = true;
    }

    private void FixedUpdate()
    {
        if(getQ)
        {
            getQ = false;
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            //设置面板
            SetUpQuestList();
            if(!isOpen)
                toolTip.gameObject.SetActive(false);
        }
    }

    public void SetUpQuestList()
    {
        content.text = string.Empty;
        foreach (Transform item in questList)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in requireList)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rewardList)
        {
            Destroy(item.gameObject);
        }
        foreach (var task in QuestManager.Instance.questTasks)
        {
            var p = Instantiate(questButton, questList);
            p.data = task.questData;
            p.questDescription = content;
            if (task.questData.isCompleted)
                p.questName.text = task.questData.questName + " (完成) ";
            else
                p.questName.text = task.questData.questName;
        }
    }

    public void SetUpRequireList(QuestData_SO data)
    {
        foreach (Transform item in requireList)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in data.requireLists)
        {
            var p = Instantiate(questRequire, requireList);
            if(data.isFinished)
                p.SetUpRequire(item.requireName);
            else
                p.SetUpRequire(item.requireName, item.currentNum, item.requireNum);
        }
    }

    public void SetUpRewardList(QuestData_SO data)
    {
        foreach (Transform item in rewardList)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in data.inventoryItems)
        {
            var p = Instantiate(questReward, rewardList);
            p.SetUpItemUI(item.ItemData, item.amount);
        }
    }

}
