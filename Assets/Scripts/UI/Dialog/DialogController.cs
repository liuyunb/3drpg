using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public DialogData_SO dialogData;



    bool canTalk;
    //·ÀÖ¹ÔÙ´Îµã»÷
    bool canPoint;

    private void Start()
    {
        dialogData.InitializeIndex();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && dialogData != null)
        {
            canTalk = true;
            //canPoint = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && dialogData != null)
        {
            canTalk = false;
            DialogUI.Instance.dialogPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if(canTalk && Input.GetMouseButtonDown(1) && !DialogUI.Instance.dialogPanel.activeSelf)
        {
            if (dialogData != null && dialogData.dialog.Count > 0)
            {
                OpenDialog();
            }
            //canTalk = false;
        }
    }


    public void OpenDialog()
    {
        DialogUI.Instance.UpdateDialogData(dialogData);
        DialogUI.Instance.UpdateDialog(dialogData.dialog[0]);
    }
}
