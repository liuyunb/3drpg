using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogController))]
public class QuestDialogController : MonoBehaviour
{
    DialogController dialogController;

    public DialogData_SO startDialog;
    public DialogData_SO processDialog;
    public DialogData_SO completeDialog;
    public DialogData_SO finishDialog;

    public QuestData_SO currentData;

    public bool isStarted { get
        {
            if(currentData != null && QuestManager.Instance.HasTask(currentData))
                return currentData.isStarted;
            else 
                return false;
        }
    }
    public bool isCompleted
    {
        get
        {
            if (currentData != null && QuestManager.Instance.HasTask(currentData))
                return currentData.isCompleted;
            else
                return false;
        }
    }
    public bool isFinished
    {
        get
        {
            if (currentData != null && QuestManager.Instance.HasTask(currentData))
                return currentData.isFinished;
            else
                return false;
        }
    }


    private void Awake()
    {
        dialogController = GetComponent<DialogController>();
    }
    private void Start()
    {
        dialogController.dialogData = startDialog;
    }

    private void Update()
    {
        currentData = dialogController.dialogData.GetTask();
        if (isStarted)
        {
            if (isCompleted)
                dialogController.dialogData = completeDialog;
            else
                dialogController.dialogData = processDialog;
        }

        if (isFinished)
            dialogController.dialogData = finishDialog;
    }
}
