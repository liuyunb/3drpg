using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogUI : SingleTon<DialogUI>
{
    [Header("Basic")]
    public Image icon;

    public Text text;

    public Button next;

    [Header("Dialog")]
    public GameObject dialogPanel;

    [Header("Option")]
    public RectTransform optionPanel;
    public GameObject optionPrefab;

    public DialogData_SO currentData;
    public int currentIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        next.onClick.AddListener(ContinueDialog);
    }

    public void ContinueDialog()
    {
        if(currentIndex < currentData.dialog.Count)
        {
            UpdateDialog(currentData.dialog[currentIndex]);
        }
        else
            dialogPanel.SetActive(false);
    }

    public void UpdateDialogData(DialogData_SO data)
    {
        dialogPanel.SetActive(true);
        currentData = data;
        currentIndex = 0;
    }

    public void UpdateDialog(DialogPiece piece)
    {
        if(piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
            icon.enabled = false;

        text.text = "";
        text.DOText(piece.Text, 1f);

        if(piece.options.Count == 0)
        {
            next.enabled = true;
            next.interactable = true;
            next.transform.GetChild(0).gameObject.SetActive(true);
            optionPanel.gameObject.SetActive(false);
        }
        else
        {
            next.interactable = false;
            next.transform.GetChild(0).gameObject.SetActive(false);
            CreateOptions(piece);
        }

        currentIndex++;
    }

    public void CreateOptions(DialogPiece piece)
    {
        optionPanel.gameObject.SetActive(true);
        for(int i = 0;i < optionPanel.childCount;i++)
        {
            Destroy(optionPanel.GetChild(i).gameObject);
        }

        for(int i = 0; i < piece.options.Count;i++)
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.GetComponent<OptionUI>()?.UpdateOption(piece,piece.options[i]);
        }

    }
}
