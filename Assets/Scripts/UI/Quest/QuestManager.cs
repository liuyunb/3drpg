using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : SingleTon<QuestManager>
{
    [System.Serializable]
    public class QuestTask
    {
        public QuestData_SO questData;
        public bool isStarted {get {  return questData.isStarted; }set {   questData.isStarted = value;    } }

        public bool isCompleted{   get { return questData.isCompleted; }set { questData.isCompleted = value; }}

        public bool isFinished{   get { return questData.isFinished; }set { questData.isFinished = value; }}

    }

    public List<QuestTask> questTasks = new List<QuestTask>();
    private void Start()
    {
        LoadData();
    }
    public void LoadData()
    {
        var count = PlayerPrefs.GetInt("taskCount");
        for(int i = 0;i < count;i++)
        {
            var newData = ScriptableObject.CreateInstance<QuestData_SO>();
            SaveManager.Instance.Load(newData, "task" + i);
            questTasks.Add(new QuestTask { questData = newData });
        }
    }

    public void SaveData()
    {
        var count = questTasks.Count();
        PlayerPrefs.SetInt("taskCount", count);
        for(int i = 0;i < count;i++)
        {
            SaveManager.Instance.Save(questTasks[i].questData, "task" + i);
        }
    }
    public bool HasTask(QuestData_SO data)
    {
        if(data != null)
            return questTasks.Any(t => t.questData.questName == data.questName);

        return false;
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        foreach (var task in questTasks)
        {
            if(task.questData.questName == data.questName)
                return task;
        }
        return null;
    }

    public void UpdateQuestProcess(string requireName,int amount)
    {
        foreach (var task in questTasks)
        {
            var quest = task.questData.requireLists.Find(r => r.requireName == requireName);
            if(quest != null)
                quest.currentNum = Mathf.Max(quest.currentNum + amount,0);

            task.questData.CheckQuestProcess();
        }
    }
}
