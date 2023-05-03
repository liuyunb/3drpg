using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequire : MonoBehaviour
{
    private Text requireItem;
    private Text currentProcess;

    private void Awake()
    {
        requireItem = GetComponent<Text>();
        currentProcess = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetUpRequire(string item,int currentNum,int requireNum)
    {
        requireItem.text = item;
        currentProcess.text = currentNum + "/" + requireNum;
    }
    public void SetUpRequire(string item)
    {
        requireItem.text = item;
        currentProcess.text = "Íê³É";
        requireItem.color = Color.grey;
        currentProcess.color = Color.grey;
    }
}
