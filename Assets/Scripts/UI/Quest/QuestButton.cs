using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    public Text questName;

    public QuestData_SO data;

    public Text questDescription;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SetUpDescription);
    }

    public void SetUpDescription()
    {
        questDescription.text = data.questDescription;
        QuestUI.Instance.SetUpRequireList(data);
        QuestUI.Instance.SetUpRewardList(data);
    }

}
