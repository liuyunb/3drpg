using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text text;

    Button button;

    string nextId;

    DialogPiece currentPiece;

    bool takeQuest;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnOptionClick);
    }

    public void UpdateOption(DialogPiece piece,DialogOption option)
    {
        currentPiece = piece;
        text.text = option.text;
        nextId = option.targetId;
        takeQuest = option.takeQuest;
    }

    public void OnOptionClick()
    {
        if(nextId == "")
            DialogUI.Instance.dialogPanel.SetActive(false);
        else if(DialogUI.Instance.currentData.dialogIndex.ContainsKey(nextId))
        {
            DialogUI.Instance.UpdateDialog(DialogUI.Instance.currentData.dialogIndex[nextId]);
        }

        if(currentPiece.questData != null)
        {
            var questTask = new QuestManager.QuestTask { questData = Instantiate(currentPiece.questData) };  
            if(takeQuest)
            {
                if(QuestManager.Instance.HasTask(questTask.questData))
                {
                    //检查任务进度
                    if(questTask.questData.isCompleted)
                    {
                        questTask.questData.GiveRewards();
                        questTask.questData.isFinished = true;
                    }
                }
                else
                {
                    QuestManager.Instance.questTasks.Add(questTask);
                    questTask.isStarted = true;

                    foreach (var require in questTask.questData.requireLists)
                    {
                        InventoryManager.Instance.CheckQuestItemInBag(require.requireName);
                    }
                }
            }
        }
    }

}
